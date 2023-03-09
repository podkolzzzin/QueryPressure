using Npgsql;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Postgres.Core;

public class PostgresDbConnectionProvider : DbConnectionProviderBase<NpgsqlConnection>
{
  public PostgresDbConnectionProvider(string connectionString) : base(connectionString)
  {
  }

  public override async Task<IServerInfo> GetServerInfoAsync(CancellationToken cancellationToken)
  {
    await using var connection = await CreateOpenConnectionAsync(ConnectionString, cancellationToken);
    await using var cmd = connection.CreateCommand();
    cmd.CommandText = "SELECT version()";
    var result = await cmd.ExecuteScalarAsync(cancellationToken);
    return new ServerInfo(result.ToString());
  }

  protected override async Task<NpgsqlConnection> CreateOpenConnectionAsync(string connectionString, CancellationToken cancellationToken)
  {
    var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync(cancellationToken);
    return connection;
  }
}
