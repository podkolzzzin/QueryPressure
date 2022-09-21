using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.LoadProfiles;

public class TargetThroughputLoadProfile : IProfile
{
    private readonly TimeSpan _delay;
    private DateTime? _nextExecution;
    
    public TargetThroughputLoadProfile(int targetRPS)
    {
        _delay = TimeSpan.FromMilliseconds(1000f / targetRPS);
    }
    
    public async Task WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        if (_nextExecution == null || now > _nextExecution)
        {
            _nextExecution = now + _delay;
        }
        else
        {
            var delta = _nextExecution.Value - now;
            _nextExecution += _delay;
            await Task.Delay(delta, cancellationToken);
        }
    }

    public Task OnQueryExecutedAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}