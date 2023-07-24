using QueryPressure.Core.Interfaces;
using QueryPressure.UI.Hubs;
using QueryPressure.UI.Inderfaces;

namespace QueryPressure.UI.Services;

public class ExecutionEventPublisher 
{
  private readonly IHubService<DashboardHub> _hubService;
 
  public ExecutionEventPublisher(IHubService<DashboardHub> hubService)
  {
    _hubService = hubService;
  }

  public async Task PublishMetricsAsync(Guid executionId, IEnumerable<IMetric> metrics, CancellationToken cancellationToken)
  {
    foreach (var metric in metrics)
    {
      await _hubService.SendExecutionMetric(executionId, metric, cancellationToken);
    }
  }
  
  public Task PublishCompletionStatusAsync(Guid executionId, bool isSuccess, string? message, CancellationToken cancellationToken)
  {
    return _hubService.SendCompletionStatus(executionId, isSuccess, message, cancellationToken);
  }
}
