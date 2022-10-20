using QueryPressure.App.Factories;
using QueryPressure.App.Interfaces;
using QueryPressure.App.LimitCreators;
using QueryPressure.Core.Interfaces;
using QueryPressure.Postgres.App;
using QueryPressure.Postgres.Core;
using Xunit;

namespace QueryPressure.Tests;

public class ConnectionProviderFactoryTests
{
  private readonly SettingsFactory<IConnectionProvider> _factory;
  
  public ConnectionProviderFactoryTests()
  {
    _factory = new SettingsFactory<IConnectionProvider>("connection", new ICreator<IConnectionProvider>[]
    {
      new PostgesConnectionProviderCreator()
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
    Assert.IsType<PostgresConnectionProvider>(provider);
  }
}