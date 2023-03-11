using Microsoft.Data.SqlClient;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.SqlServer.Core
{
  public class SqlServerConnectionProvider : DbConnectionProviderBase<SqlConnection>
  {

    public SqlServerConnectionProvider(string connectionString) : base(connectionString)
    { }

    protected override async Task<SqlConnection> CreateOpenConnectionAsync(string connectionString, CancellationToken cancellationToken)
    {
      var connection = new SqlConnection(connectionString);
      await connection.OpenAsync(cancellationToken);
      return connection;
    }

    override public async Task<IServerInfo> GetServerInfoAsync(CancellationToken cancellationToken)
    {
      await using SqlConnection? connection = await CreateOpenConnectionAsync(ConnectionString, cancellationToken);
      await using SqlCommand? cmd = connection.CreateCommand();
      cmd.CommandText = "SELECT @@VERSION";
      object? result = await cmd.ExecuteScalarAsync(cancellationToken);
      return new ServerInfo(result.ToString());
    }
  }
}
