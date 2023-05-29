namespace QueryPressure.WinUI.Common.Observer;

public interface ISubject<in TPayload>
{
  void Notify(object sender, TPayload payload);
  void SetCurrentValue(TPayload payload);
}
