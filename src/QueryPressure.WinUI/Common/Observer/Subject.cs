namespace QueryPressure.WinUI.Common.Observer;

public delegate void OnSubjectNext<in TPayload>(TPayload value);

public class Subject<TPayload> : ISubject<TPayload>, IObservableItem<TPayload>, IDisposable
{
  private readonly IDictionary<string, OnSubjectNext<TPayload>> _subscriptions;

  public Subject()
  {
    _subscriptions = new Dictionary<string, OnSubjectNext<TPayload>>();
  }

  public TPayload? CurrentValue { get; private set; }

  public void Notify(TPayload payload)
  {
    SetCurrentValue(payload);
    foreach (var subscription in _subscriptions)
    {
      subscription.Value.Invoke(payload);
    }
  }

  public void SetCurrentValue(TPayload payload)
  {
    CurrentValue = payload;
  }

  private void Unsubscribe(string key)
  {
    if (!_subscriptions.ContainsKey(key))
    {
      throw new InvalidOperationException($"Failed to unsubscribe. Subscription with the key '{key}' not exist");
    }

    _subscriptions.Remove(key);
  }

  public ISubscription SubscribeWithKey(OnSubjectNext<TPayload> onValueChanged, string key = "")
  {
    if (_subscriptions.ContainsKey(key))
    {
      throw new InvalidOperationException($"Failed to subscribe. Subscription with the same key '{key}' already exist");
    }

    _subscriptions.Add(key, onValueChanged);

    return new Subscription(key, () => Unsubscribe(key));
  }

  public void Unsubscribe(ISubscription subscription) => Unsubscribe(subscription.Key);

  public void Dispose()
  {
#if DEBUG
    if (!_subscriptions.Any())
    {
      return;
    }

    var subs = string.Join(", ", _subscriptions.Select(x => x.Key));
    throw new InvalidOperationException($"Before Shutdown you should unsubscribe the {typeof(TPayload).Name}: {subs}");
#endif
  }
}
