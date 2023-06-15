using Microsoft.AspNetCore.SignalR;

namespace QueryPressure.UI;

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
  public async Task NotifyExecutionAsync<T>(string executionId, T data)
  {
    await _hubContext.Clients.Group(executionId).SendAsync("live-metrics", data);
  }
}

public interface IHubService<THub> where THub : Hub
{
  Task NotifyExecutionAsync<T>(string executionId, T data);
}
