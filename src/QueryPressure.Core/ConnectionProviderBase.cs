using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Requirements;

namespace QueryPressure.Core;

public abstract class ConnectionProviderBase<T> : IConnectionProvider
{
  private readonly string _connectionString;

  public ConnectionProviderBase(string connectionString)
  {
    _connectionString = connectionString;
  }

  protected abstract Task<T> CreateOpenConnectionAsync(string connectionString, CancellationToken cancellationToken);

  protected abstract IExecutable CreateExecutor(IScript script, IConnectionPool<T> pool);

  public async Task<IExecutable> CreateExecutorAsync(IScriptSource scriptSource, ConnectionRequirement connectionRequirement, CancellationToken cancellationToken)
  {
    var script = await scriptSource.GetScriptAsync(cancellationToken);
    var connections = new T[connectionRequirement.ConnectionCount];
    for (int i = 0; i < connections.Length; i++)
    {
      connections[i] = await CreateOpenConnectionAsync(_connectionString, cancellationToken);
    }

    return CreateExecutor(script, new ConnectionPool<T>(connections));
  }
}
