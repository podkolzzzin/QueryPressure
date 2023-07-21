namespace QueryPressure.UI;

public class ExecutionStatusWatcher : BackgroundInfiniteService
{
  private readonly IExecutionStore _executionStore;
  private readonly ExecutionEventPublished _eventPublished;

  public ExecutionStatusWatcher(IExecutionStore executionStore, ExecutionEventPublished eventPublished)
  {
    _executionStore = executionStore;
    _eventPublished = eventPublished;
  }

  protected override Task ExecuteRepeatedJobAsync(CancellationToken stoppingToken)
  {
    return Parallel.ForEachAsync(_executionStore.RunningExecutions, stoppingToken, async (execution, token) =>
    {
      if (token.IsCancellationRequested)
        return;

      await Task.WhenAny(execution.ExecutionTask, Task.Delay(100, token));

      var metrics = execution.MetricProviders.SelectMany(x => x.GetMetrics());
      await _eventPublished.PublishMetricsAsync(execution.Id, metrics, stoppingToken);
    });
  }
}
