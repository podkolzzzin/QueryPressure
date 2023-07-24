using Microsoft.AspNetCore.SignalR;
using QueryPressure.Core.Interfaces;
using QueryPressure.UI.Inderfaces;

namespace QueryPressure.UI.Services;

public class HubService<THub> : IHubService<THub> where THub : Hub
{
  private readonly IHubContext<THub> _hubContext;

  public HubService(IHubContext<THub> hubContext)
  {
    _hubContext = hubContext;
  }

  public async Task SendMessageToAllAsync<T>(string method, T data)
  {
    await _hubContext.Clients.All.SendAsync($"{typeof(THub).Name}.{method}", data);
  }

  public Task SendExecutionMetric(Guid executionId, IMetric metric, CancellationToken cancellationToken)
  {
    return _hubContext.Clients.Group(executionId.ToString()).SendAsync(metric.Name, metric.Value, cancellationToken);
  }

  public Task SendCompletionStatus(Guid executionId, bool isCompletedSuccessfully, string? message, CancellationToken cancellationToken)
  {
    // TODO: it's POC version, refactor it later
    return _hubContext.Clients.Group(executionId.ToString())
      .SendAsync("execution-completed", new
      {
        IsCompletedSuccessfully = isCompletedSuccessfully,
        Message = message,
      }, cancellationToken);
  }
}
