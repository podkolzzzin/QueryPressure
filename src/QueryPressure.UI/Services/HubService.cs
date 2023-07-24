using Microsoft.AspNetCore.SignalR;
using QueryPressure.Core.Interfaces;
using QueryPressure.UI.Interfaces;

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

  public async Task SendExecutionMetricAsync(Guid executionId, IVisualization metric, CancellationToken cancellationToken)
  {
    await _hubContext.Clients.Group(executionId.ToString()).SendAsync("live-metrics", metric, cancellationToken);
  }

  public Task SendCompletionStatusAsync(Guid executionId, bool isCompletedSuccessfully, string? message, CancellationToken cancellationToken)
  {
    // TODO: it's POC version, refactor it later
    return _hubContext.Clients.All.SendAsync("execution-completed", new
    {
      IsCompletedSuccessfully = isCompletedSuccessfully,
      Message = message,
    }, cancellationToken);
  }
}
