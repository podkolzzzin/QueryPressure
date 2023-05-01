using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.Services.Subscriptions;

public class ObservableModel : IObservableItem<IModel>
{
  private readonly ISubscriptionManager _subscriptionManager;
  private readonly IObservableItem<IModel> _observable;

  public ObservableModel(ISubscriptionManager subscriptionManager, IObservableItem<IModel> observable)
  {
    _subscriptionManager = subscriptionManager;
    _observable = observable;
  }

  public ISubscription SubscribeWithKey(OnSubjectNext<IModel> onValueChanged, string key = "")
  {
    return _observable.SubscribeWithKey(onValueChanged, key);
  }

  public void Unsubscribe(ISubscription subscription)
  {
    _subscriptionManager.Remove(subscription.Key);
    _observable.Unsubscribe(subscription);
  }

  public IModel? CurrentValue => _observable.CurrentValue;
}
