using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.LoadProfiles;

public class SequentialLoadProfile : IProfile, IExecutionHook
{
  private TaskCompletionSourceWithCancellation? _taskCompletionSource;

  public async Task WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
  {
    if (_taskCompletionSource != null)
      await _taskCompletionSource.Task;

    _taskCompletionSource = new(cancellationToken);
  }

  public Task OnQueryExecutedAsync(ExecutionResult _, CancellationToken cancellationToken)
  {
    if (_taskCompletionSource == null)
      throw new InvalidOperationException(
          $"{nameof(OnQueryExecutedAsync)} is called before first {nameof(WhenNextCanBeExecutedAsync)} called.");

    var tcs = _taskCompletionSource;
    _taskCompletionSource = null;

    tcs.SetResult();
    tcs.Dispose();


    return Task.CompletedTask;
  }
}
