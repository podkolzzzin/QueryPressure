using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.LoadProfiles;

public class LimitedConcurrencyLoadProfile : IProfile
{
    private readonly SemaphoreSlim _semaphore;
    
    public LimitedConcurrencyLoadProfile(int limit)
    {
        _semaphore = new SemaphoreSlim(limit);
    }
    public async Task<IExecutionDescriptor> WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
    {
            
        await _semaphore.WaitAsync(cancellationToken);

        return ExecutionDescriptor.Empty;
    }

    public Task OnQueryExecutedAsync(IExecutionDescriptor _, CancellationToken cancellationToken)
    {
        _semaphore.Release();
        return Task.CompletedTask;
    }
}