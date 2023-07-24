using System.Collections.Immutable;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.UI.Models;

namespace QueryPressure.UI.Inderfaces;

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
