using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.LoadProfiles; 

public class LimitedConcurrencyWithDelayLoadProfile : IProfile
{
    private readonly TimeSpan _delay;
    private readonly LimitedConcurrencyLoadProfile _internal;

    public LimitedConcurrencyWithDelayLoadProfile(int limit, TimeSpan delay)
    {
        _delay = delay;
        _internal = new (limit);
    }

    public Task WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
    {
        return _internal.WhenNextCanBeExecutedAsync(cancellationToken);
    }

    public async Task OnQueryExecutedAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(_delay, cancellationToken);
        await _internal.OnQueryExecutedAsync(cancellationToken);
    }
}
