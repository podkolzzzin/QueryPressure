namespace QueryPressure.WinUI.Common.Observer;

public interface ISubject<in TPayload>
{
  void Notify(TPayload payload);
}
