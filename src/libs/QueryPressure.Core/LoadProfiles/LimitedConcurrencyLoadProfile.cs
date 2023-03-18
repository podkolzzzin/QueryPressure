using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Requirements;

namespace QueryPressure.Core.LoadProfiles;

public class LimitedConcurrencyLoadProfile : IProfile, IExecutionHook
{
  private readonly int _limit;
  private readonly SemaphoreSlim _semaphore;

  public LimitedConcurrencyLoadProfile(int limit)
  {
    _limit = limit;
    _semaphore = new SemaphoreSlim(limit);
  }
  public async Task WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
  {
    await _semaphore.WaitAsync(cancellationToken);
  }


  public IRequirement[] Requirements => new[] { new ConnectionRequirement(_limit) };

  public Task OnQueryExecutedAsync(ExecutionResult result, CancellationToken cancellationToken)
  {
    _semaphore.Release();
    return Task.CompletedTask;
  }
}
