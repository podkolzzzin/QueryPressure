using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.LoadProfiles;

public class SequentialWithDelayLoadProfile : IProfile
{
    private readonly TimeSpan _delay;
    private readonly IProfile _profile;
    private DateTime? _nextExecution;
    
    public SequentialWithDelayLoadProfile(TimeSpan delay)
    {
        _delay = delay;
        _profile = new SequentialLoadProfile();
    }
    
    public async Task WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
    {
        await _profile.WhenNextCanBeExecutedAsync(cancellationToken);
        var now = DateTime.Now;
        if (_nextExecution != null && now < _nextExecution)
            await Task.Delay(_nextExecution.Value - now, cancellationToken);
    }

    public async Task OnQueryExecutedAsync(CancellationToken cancellationToken)
    {
        await _profile.OnQueryExecutedAsync(cancellationToken);
        _nextExecution = DateTime.Now + _delay;
    }
}