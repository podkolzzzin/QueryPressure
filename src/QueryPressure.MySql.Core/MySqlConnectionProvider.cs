using MySqlConnector;
using QueryPressure.Core;

namespace QueryPressure.MySql.Core
{
  public class MySqlConnectionProvider : ConnectionProviderBase<MySqlConnection>
  {
    public MySqlConnectionProvider(string connectionString) : base(connectionString)
    {
    }

    protected override async Task<MySqlConnection> CreateOpenConnectionAsync(
      string connectionString,
      CancellationToken cancellationToken)
    {
      var connection = new MySqlConnection(connectionString);
      await connection.OpenAsync(cancellationToken);
      return connection;
    }
  }
}
