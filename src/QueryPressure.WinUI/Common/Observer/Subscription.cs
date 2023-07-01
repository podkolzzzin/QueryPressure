namespace QueryPressure.WinUI.Common.Observer;

public record Subscription(string Key, Action Unsubscribe) : ISubscription
{
  public void Dispose()
  {
    Unsubscribe.Invoke();
  }
}
