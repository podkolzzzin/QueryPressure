namespace QueryPressure.Core;

internal class TaskCompletionSourceWithCancellation : TaskCompletionSource, IDisposable
{
  private readonly CancellationToken _cancellationToken;
  private readonly IDisposable _registration;

  public TaskCompletionSourceWithCancellation(CancellationToken cancellationToken)
  {
    _cancellationToken = cancellationToken;
    _registration = cancellationToken.Register((_, token) => TrySetCanceled(token), false);
  }
  public void Dispose() => _registration.Dispose();
}
