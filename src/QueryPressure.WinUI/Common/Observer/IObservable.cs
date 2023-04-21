using System.Runtime.CompilerServices;

namespace QueryPressure.WinUI.Common.Observer;

public interface IObservableItem<out TPayload>
{
  ISubscription Subscribe(OnSubjectNext<TPayload> onValueChanged, [CallerFilePath] string key = "");
  void Unsubscribe(ISubscription subscription);
}
