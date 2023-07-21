using QueryPressure.UI.Inderfaces;
using QueryPressure.UI.Services;

namespace QueryPressure.UI.HostedServices;

public class ExecutionStatusWatcher : BackgroundInfiniteService
{
  private readonly IExecutionStore _executionStore;
  private readonly ExecutionEventPublisher _eventPublisher;

  public ExecutionStatusWatcher(IExecutionStore executionStore, ExecutionEventPublisher eventPublisher)
  {
    _executionStore = executionStore;
    _eventPublisher = eventPublisher;
  }

  protected override Task ExecuteRepeatedJobAsync(CancellationToken stoppingToken)
  {
    return Parallel.ForEachAsync(_executionStore.RunningExecutions, stoppingToken, async (execution, token) =>
    {
      if (token.IsCancellationRequested)
        return;

      await Task.WhenAny(execution.ExecutionTask, Task.Delay(100, token));

      var metrics = execution.MetricProviders.SelectMany(x => x.GetMetrics());
      await _eventPublisher.PublishMetricsAsync(execution.Id, metrics, stoppingToken);
    });
  }
}
