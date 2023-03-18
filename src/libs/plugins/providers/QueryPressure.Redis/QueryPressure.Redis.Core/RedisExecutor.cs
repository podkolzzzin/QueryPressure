using QueryPressure.Core.Interfaces;
using QueryPressure.Core.ScriptSources;
using StackExchange.Redis;

namespace QueryPressure.Redis.Core
{
  public sealed class RedisExecutor : IExecutable
  {
    private readonly IConnectionPool<ConnectionMultiplexer> _connections;
    private readonly TextScript _script;

    public RedisExecutor(IScript script, IConnectionPool<ConnectionMultiplexer> connections)
    {
      if (script is not TextScript textScript)
        throw new ApplicationException("The only supported script type is TextScript");
      _connections = connections;
      _script = textScript;
    }

    public void Dispose() => _connections.Dispose();

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
      using var holder = _connections.UseConnection();
      var database = holder.Connection.GetDatabase();
      _ = await database.ScriptEvaluateAsync(_script.Text);
    }
  }
}
