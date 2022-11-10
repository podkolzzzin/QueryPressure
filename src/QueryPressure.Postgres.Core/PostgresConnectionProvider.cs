using Microsoft.Extensions.ObjectPool;
using Npgsql;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Requirements;

namespace QueryPressure.Postgres.Core;

public class PostgresConnectionProvider : IConnectionProvider
{
  private readonly string _connectionString;
  
  public PostgresConnectionProvider(string connectionString)
  {
    _connectionString = connectionString;
  }

  public async Task<IExecutable> CreateExecutorAsync(IScriptSource scriptSource, ConnectionRequirement connectionRequirement, CancellationToken cancellationToken)
  {
    var script = await scriptSource.GetScriptAsync(cancellationToken);
    var connections = new NpgsqlConnection[connectionRequirement.ConnectionCount];
    for (int i = 0; i < connections.Length; i++)
    {
      connections[i] = new NpgsqlConnection(_connectionString);
      await connections[i].OpenAsync(cancellationToken);
    }
    
    return new PostgresExecutor(script, new ConnectionsProvider<NpgsqlConnection>(connections));
  }
}