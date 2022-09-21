using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.LoadProfiles;

public class LimitedConcurrencyLoadProfile : IProfile
{
    private readonly SemaphoreSlim _semaphore;
    
    public LimitedConcurrencyLoadProfile(int limit)
    {
        _semaphore = new SemaphoreSlim(limit);
    }
    public async Task WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);
    }

    public Task OnQueryExecutedAsync(CancellationToken cancellationToken)
    {
        _semaphore.Release();
        return Task.CompletedTask;
    }
}