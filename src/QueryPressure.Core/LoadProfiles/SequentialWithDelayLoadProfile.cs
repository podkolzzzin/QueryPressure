using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.LoadProfiles;

public class SequentialWithDelayLoadProfile : IProfile, IExecutionHook
{
    private readonly TimeSpan _delay;
    private readonly SequentialLoadProfile _profile;
    private DateTime? _nextExecution;
    
    public SequentialWithDelayLoadProfile(TimeSpan delay)
    {
        _delay = delay;
        _profile = new ();
    }
    
    public async Task WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
    {
        await _profile.WhenNextCanBeExecutedAsync(cancellationToken);
        var now = DateTime.Now;
        if (_nextExecution != null && now < _nextExecution)
            await Task.Delay(_nextExecution.Value - now, cancellationToken);
    }

    public async Task OnQueryExecutedAsync(ExecutionResult _, CancellationToken cancellationToken)
    {
        await _profile.OnQueryExecutedAsync(_, cancellationToken);
        _nextExecution = DateTime.Now + _delay;
    }
}