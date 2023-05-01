using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.Services.Subscriptions;

public interface ISubscriptionManager
{
  IObservableItem<IModel> On(ModelAction action, IModel model);
  void Notify(ModelAction action, IModel model);
  void Remove(string key);
}
