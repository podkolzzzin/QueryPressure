using Microsoft.Data.SqlClient;
using QueryPressure.Core;

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
  }
}
