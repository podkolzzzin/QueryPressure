using System.Collections.Concurrent;
using System.Collections.Immutable;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.UI.Inderfaces;
using QueryPressure.UI.Models;

namespace QueryPressure.UI.Services;

public class ExecutionStore : IExecutionStore
{
  private readonly ConcurrentDictionary<Guid, Execution> _executions = new();
  public ImmutableArray<Execution> RunningExecutions => _executions.Values
      .Where(x => !x.ExecutionTask.IsCompleted)
      .ToImmutableArray();

  public ImmutableArray<Execution> CompletedExecutions => _executions.Values
    .Where(x => x.ExecutionTask.IsCompleted)
    .ToImmutableArray();


  public Task<Guid> SaveAsync(Task executionTask,
    IExecutionResultStore executionResultStore,
    IReadOnlyCollection<ILiveMetricProvider> liveMetricProviders,
    CancellationTokenSource cts)
  {
    var executionId = Guid.NewGuid();
    var execution = new Execution(executionId, executionTask, executionResultStore, liveMetricProviders, cts);

    _executions.TryAdd(executionId, execution);
    
    return Task.FromResult(executionId);
  }

  public Task CancelAsync(Guid executionId)
  {
    var execution = _executions.GetValueOrDefault(executionId);
    if (execution is not null && !execution.ExecutionTask.IsCompleted)
      execution.CancellationTokenSource.Cancel();
    
    return Task.CompletedTask;
  }

  public Task RemoveAsync(Guid executionId)
  {
    _executions.TryRemove(executionId, out _);
    return Task.CompletedTask;
  }
}
