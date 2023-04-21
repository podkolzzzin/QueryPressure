using System.Runtime.CompilerServices;

namespace QueryPressure.WinUI.Common.Observer;

public delegate void OnSubjectNext<in TPayload>(TPayload value);

public class Subject<TPayload> : ISubject<TPayload>, IObservableItem<TPayload>, IDisposable
{
  private readonly IDictionary<string, OnSubjectNext<TPayload>> _subscriptions;

  public Subject()
  {
    _subscriptions = new Dictionary<string, OnSubjectNext<TPayload>>();
  }

  public void Notify(TPayload payload)
  {
    foreach (var subscription in _subscriptions)
    {
      subscription.Value.Invoke(payload);
    }
  }

  public ISubscription Subscribe(OnSubjectNext<TPayload> onLanguageChanged, [CallerFilePath] string key = "")
  {
    if (_subscriptions.ContainsKey(key))
    {
      throw new InvalidOperationException($"Failed to subscribe. Subscription with the same key '{key}' already exist");
    }

    _subscriptions.Add(key, onLanguageChanged);

    return new Subscription(key, () => Unsubscribe(key));
  }

  private void Unsubscribe(string key)
  {
    if (!_subscriptions.ContainsKey(key))
    {
      throw new InvalidOperationException($"Failed to unsubscribe. Subscription with the key '{key}' not exist");
    }

    _subscriptions.Remove(key);
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
