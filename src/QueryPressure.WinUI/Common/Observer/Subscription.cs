namespace QueryPressure.WinUI.Common.Observer;

public readonly record struct Subscription(string Key, Action Unsubscribe) : ISubscription
{
  public void Dispose()
  {
    Unsubscribe.Invoke();
  }
}
