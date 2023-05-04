using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.Services.Subscriptions;

public class ObservableModel : IObservableItem<IModel>
{
  private readonly ISubscriptionManager _subscriptionManager;
  private readonly string _key;
  private readonly IObservableItem<IModel> _observable;

  public ObservableModel(ISubscriptionManager subscriptionManager, string key, IObservableItem<IModel> observable)
  {
    _subscriptionManager = subscriptionManager;
    _key = key;
    _observable = observable;
  }

  public ISubscription SubscribeWithKey(OnSubjectNext<IModel> onValueChanged, string _)
  {
    var subscription = _observable.SubscribeWithKey(onValueChanged, _key);
    return new Subscription(_key, () =>
    {
      _subscriptionManager.Remove(_key);
      subscription.Dispose();
    });
  }

  public void Unsubscribe(ISubscription subscription)
  {
    throw new NotImplementedException();
  }

  public IModel? CurrentValue => _observable.CurrentValue;
}
