using QueryPressure.Core;
using QueryPressure.Core.Interfaces;
using StackExchange.Redis;

namespace QueryPressure.Redis.Core
{
  public sealed class RedisConnectionProvider : ConnectionProviderBase<ConnectionMultiplexer>
  {
    public RedisConnectionProvider(string connectionString) : base(connectionString)
    { }

    protected override async Task<ConnectionMultiplexer> CreateOpenConnectionAsync(string connectionString, CancellationToken _)
    {
      ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync(connectionString);
      return connection;
    }

    protected override IExecutable CreateExecutor(IScript script, IConnectionPool<ConnectionMultiplexer> pool)
      => new RedisExecutor(script, pool);
  }
}
