namespace QueryPressure.Core.Interfaces;

public interface IConnectionPool<T> : IDisposable
{
  IConnectionHolder<T> UseConnection();
}

public interface IConnectionHolder<T> : IDisposable
{
  T Connection { get; }
}
