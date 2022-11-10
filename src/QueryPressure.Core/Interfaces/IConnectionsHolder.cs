namespace QueryPressure.Core.Interfaces;

public interface IConnectionsHolder<T> : IDisposable
{
  IConnectionHolder<T> UseConnection();
}

public interface IConnectionHolder<T> : IDisposable
{
  T Connection { get; }
}
