using System.Collections.Concurrent;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core;

public class ConnectionPool<T> : IConnectionPool<T>
{
  private class ConnectionHolder : IConnectionHolder<T>
  {
    private readonly ConnectionPool<T> _service;
    public T Connection { get; }

    public ConnectionHolder(T connection, ConnectionPool<T> service)
    {
      _service = service;
      Connection = connection;
    }

    public void Dispose()
    {
      _service.FinalizeConnection(Connection);
    }
  }

  private readonly ConcurrentBag<T> _connections;

  public ConnectionPool(IEnumerable<T> connections)
  {
    _connections = new ConcurrentBag<T>(connections);
  }

  public IConnectionHolder<T> UseConnection()
  {
    if (!_connections.TryTake(out var result))
      throw new ApplicationException("No available connections");
    return new ConnectionHolder(result, this);
  }

  private void FinalizeConnection(T connection) => _connections.Add(connection);

  public void Dispose()
  {
    while (!_connections.IsEmpty)
    {
      if (_connections.TryTake(out var c))
        (c as IDisposable)?.Dispose();
    }
  }
}
