using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.LoadProfiles;

public class SequentialLoadProfileWithDelay : IProfile
{
    private readonly TimeSpan _delay;
    private readonly IProfile _profile;
    private DateTime? _nextExecution;
    
    public SequentialLoadProfileWithDelay(TimeSpan delay)
    {
        _delay = delay;
        _profile = new SequentialLoadProfile();
    }
    
    public async Task<bool> WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
    {
        var result = await _profile.WhenNextCanBeExecutedAsync(cancellationToken);
        var now = DateTime.Now;
        if (_nextExecution != null && now < _nextExecution)
            await Task.Delay(_nextExecution.Value - now, cancellationToken);

        return result;
    }

    public async Task OnQueryExecutedAsync(CancellationToken cancellationToken)
    {
        await _profile.OnQueryExecutedAsync(cancellationToken);
        _nextExecution = DateTime.Now + _delay;
    }
}