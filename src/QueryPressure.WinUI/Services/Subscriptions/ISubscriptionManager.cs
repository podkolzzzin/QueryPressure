using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using System.Runtime.CompilerServices;

namespace QueryPressure.WinUI.Services.Subscriptions;

public interface ISubscriptionManager
{
  IObservableItem<IModel> On(ModelAction action, IModel model, [CallerFilePath] string? caller = null);
  void Notify(object? sender, ModelAction action, IModel model);
  void Remove(SubscriptionKey key);
}
