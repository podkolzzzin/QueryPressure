using QueryPressure.Core.Interfaces;

namespace QueryPressure.Postgres.Core;

public class PostgresConnectionProvider : IConnectionProvider
{
  private readonly string _connectionString;
  
  public PostgresConnectionProvider(string connectionString)
  {
    _connectionString = connectionString;
  }
}