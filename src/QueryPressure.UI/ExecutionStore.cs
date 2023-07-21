using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Threading.Channels;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.UI;

public record Execution(Guid Id,
  Task ExecutionTask,
  IExecutionResultStore ExecutionResultStore,
  IReadOnlyCollection<ILiveMetricProvider> MetricProviders,
  CancellationTokenSource CancellationTokenSource);

public interface IExecutionStore
{
  ImmutableArray<Execution> RunningExecutions { get; }
  ImmutableArray<Execution> CompletedExecutions { get; }
  
  Task<Guid> SaveAsync(Task executionTask,
    IExecutionResultStore executionResultStore,
    IReadOnlyCollection<ILiveMetricProvider> liveMetricProviders,
    CancellationTokenSource cts);

  Task CancelAsync(Guid executionId); 
  
  Task RemoveAsync(Guid executionId);
}

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
