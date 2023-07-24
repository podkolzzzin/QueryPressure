using Microsoft.AspNetCore.SignalR;

namespace QueryPressure.UI.Hubs;

public class DashboardHub : Hub
{
  public override async Task OnConnectedAsync()
  {
    var executionId = Context.GetHttpContext()!.Request.Query["executionId"];

    if (Guid.TryParse(executionId.ToString(), out var id) && id != Guid.Empty)
      await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());

    await base.OnConnectedAsync();
  }
}
