using Microsoft.AspNetCore.SignalR;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.UI.Interfaces;

public interface IHubService<THub> where THub : Hub
{
  Task SendMessageToAllAsync<T>(string method, T data);
  Task SendExecutionMetricAsync(Guid executionId, IVisualization metric, CancellationToken cancellationToken);
  Task SendCompletionStatusAsync(Guid executionId, bool IsCompletedSuccessfully, string? message, CancellationToken cancellationToken);
}
