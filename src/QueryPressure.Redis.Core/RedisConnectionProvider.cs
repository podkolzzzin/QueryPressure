using QueryPressure.Core;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Requirements;
using StackExchange.Redis;

namespace QueryPressure.Redis.Core
{
  public sealed class RedisConnectionProvider : IConnectionProvider
  {
    private readonly string _connectionString;

    public RedisConnectionProvider(string connectionString)
    {
      _connectionString = connectionString;
    }

    private async Task<ConnectionMultiplexer> CreateOpenConnectionAsync(string connectionString)
    {
      ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync(connectionString);
      return connection;
    }

    private IExecutable CreateExecutor(IScript script, IConnectionPool<ConnectionMultiplexer> pool)
      => new RedisExecutor(script, pool);

    public async Task<IExecutable> CreateExecutorAsync(IScriptSource scriptSource, ConnectionRequirement connectionRequirement,
      CancellationToken cancellationToken)
    {
      var script = await scriptSource.GetScriptAsync(cancellationToken);
      var connections = new ConnectionMultiplexer[connectionRequirement.ConnectionCount];
      for (int i = 0; i < connections.Length; i++)
      {
        connections[i] = await CreateOpenConnectionAsync(_connectionString);
      }

      return CreateExecutor(script, new ConnectionPool<ConnectionMultiplexer>(connections));
    }
  }
}
