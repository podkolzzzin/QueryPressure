using Microsoft.AspNetCore.SignalR;

namespace QueryPressure.UI.Hubs;

public class DashboardHub : Hub
{
  public override Task OnConnectedAsync()
  {
    string executionId = Context.GetHttpContext()!.Request.Query["executionId"]!;
    if (!string.IsNullOrEmpty(executionId))
    {
      Groups.AddToGroupAsync(Context.ConnectionId, executionId);
    }
    return base.OnConnectedAsync();
  }
}
