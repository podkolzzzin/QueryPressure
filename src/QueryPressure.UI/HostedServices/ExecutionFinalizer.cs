using QueryPressure.UI.Inderfaces;
using QueryPressure.UI.Services;

namespace QueryPressure.UI.HostedServices;

public class ExecutionFinalizer : BackgroundInfiniteService
{
  private readonly IExecutionStore _executionStore;
  private readonly ExecutionEventPublisher _eventPublisher;

  public ExecutionFinalizer(IExecutionStore executionStore, ExecutionEventPublisher eventPublisher)
  {
    _executionStore = executionStore;
    _eventPublisher = eventPublisher;
  }

  protected override async Task ExecuteRepeatedJobAsync(CancellationToken stoppingToken)
  {
    foreach (var execution in _executionStore.CompletedExecutions)
    {
      await _eventPublisher.PublishCompletionStatusAsync(
        execution.Id, 
        execution.ExecutionTask.IsCompletedSuccessfully,
        execution.ExecutionTask.Exception?.Flatten().Message,
        stoppingToken);

      await _executionStore.RemoveAsync(execution.Id);
    }
  }
}
