using Microsoft.AspNetCore.SignalR;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.UI.Inderfaces;

public interface IHubService<THub> where THub : Hub
{
  Task SendMessageToAllAsync<T>(string method, T data);
  Task SendExecutionMetric(Guid executionId, IMetric metric, CancellationToken cancellationToken);
  Task SendCompletionStatus(Guid executionId, bool IsCompletedSuccessfully, string? message, CancellationToken cancellationToken);
}
