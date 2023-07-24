using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.UI.Models;

public record Execution(Guid Id,
  Task ExecutionTask,
  IExecutionResultStore ExecutionResultStore,
  IReadOnlyCollection<ILiveMetricProvider> MetricProviders,
  CancellationTokenSource CancellationTokenSource);
