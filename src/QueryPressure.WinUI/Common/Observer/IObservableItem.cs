using System.Runtime.CompilerServices;

namespace QueryPressure.WinUI.Common.Observer;

public interface IObservableItem<out TPayload>
{
  ISubscription SubscribeWithKey(OnSubjectNext<TPayload> onValueChanged, string key = "");
  ISubscription Subscribe(OnSubjectNext<TPayload> onValueChanged, [CallerFilePath] string caller = "") => SubscribeWithKey(onValueChanged, caller);
  void Unsubscribe(ISubscription subscription);
  TPayload? CurrentValue { get; }
}
