using QueryPressure.App.Factories;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.MySql.App;
using QueryPressure.MySql.Core;
using QueryPressure.Postgres.App;
using QueryPressure.Postgres.Core;
using QueryPressure.Redis.App;
using QueryPressure.Redis.Core;
using QueryPressure.SqlServer.App;
using QueryPressure.SqlServer.Core;
using Xunit;

namespace QueryPressure.Tests;

public class ConnectionProviderFactoryTests
{
  private readonly SettingsFactory<IConnectionProvider> _factory;

  public ConnectionProviderFactoryTests()
  {
    _factory = new SettingsFactory<IConnectionProvider>("connection", new ICreator<IConnectionProvider>[]
    {
      new PostgresConnectionProviderCreator(),
      new MySqlConnectionProviderCreator(),
      new RedisConnectionProviderCreator(),
      new SqlServerConnectionProviderCreator()
    });
  }

  [Fact]
  public void Create_PostgresConnectionProvider_IsCreated()
  {
    var yml = @"
connection:
  type: postgres
  arguments:
     connectionString: Host=localhost;Database=postgres;User Id=postgres;Password=postgres;";

    var provider = TestUtils.Create(_factory, yml);
    Assert.IsType<PostgresDbConnectionProvider>(provider);
  }

  [Fact]
  public void Create_MySqlConnectionProvider_IsCreated()
  {
    var yml = @"
connection:
  type: mysql
  arguments:
     connectionString: Host=localhost;Database=query_pressure_db;User Id=root;SSL Mode=None";

    var provider = TestUtils.Create(_factory, yml);
    Assert.IsType<MySqlDbConnectionProvider>(provider);
  }

  [Fact]
  public void Create_RedisConnectionProvider_IsCreated()
  {
    var yml = @"
connection:
  type: redis
  arguments:
     connectionString: localhost";

    var provider = TestUtils.Create(_factory, yml);
    Assert.IsType<RedisConnectionProvider>(provider);
  }

  [Fact]
  public void Create_SqlServerConnectionProvider_IsCreated()
  {
    var yml = @"
connection:
  type: sqlserver
  arguments:
     connectionString: Data Source=localhost,1433;Initial Catalog=master;User ID=sa;Password=Pass@word;TrustServerCertificate=True;";

    var provider = TestUtils.Create(_factory, yml);
    Assert.IsType<SqlServerConnectionProvider>(provider);
  }
}
