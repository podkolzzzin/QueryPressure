using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Requirements;

namespace QueryPressure.Core;

public abstract class ConnectionProviderBase<T> : IConnectionProvider
{
  protected string ConnectionString { get; }

  protected ConnectionProviderBase(string connectionString)
  {
    ConnectionString = connectionString;
  }

  protected abstract Task<T> CreateOpenConnectionAsync(string connectionString, CancellationToken cancellationToken);

  protected abstract IExecutable CreateExecutor(IScript script, IConnectionPool<T> pool);

  public virtual Task<IServerInfo> GetServerInfoAsync(CancellationToken cancellationToken)
  {
    // TODO: (!!!) make this method abstract!
    throw new NotImplementedException();
  }
  
  public async Task<IExecutable> CreateExecutorAsync(IScriptSource scriptSource, ConnectionRequirement connectionRequirement, CancellationToken cancellationToken)
  {
    var script = await scriptSource.GetScriptAsync(cancellationToken);
    var connections = new T[connectionRequirement.ConnectionCount];
    for (int i = 0; i < connections.Length; i++)
    {
      connections[i] = await CreateOpenConnectionAsync(ConnectionString, cancellationToken);
    }

    return CreateExecutor(script, new ConnectionPool<T>(connections));
  }
}
