namespace QueryPressure.WinUI.Common.Observer;

public interface ISubscription : IDisposable
{
  string Key { get; }
}
