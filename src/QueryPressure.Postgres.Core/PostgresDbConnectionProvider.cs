using Npgsql;
using QueryPressure.Core;

namespace QueryPressure.Postgres.Core;

public class PostgresDbConnectionProvider : DbConnectionProviderBase<NpgsqlConnection>
{
  public PostgresDbConnectionProvider(string connectionString) : base(connectionString)
  {
  }

  protected override async Task<NpgsqlConnection> CreateOpenConnectionAsync(string connectionString, CancellationToken cancellationToken)
  {
    var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync(cancellationToken);
    return connection;
  }
}
