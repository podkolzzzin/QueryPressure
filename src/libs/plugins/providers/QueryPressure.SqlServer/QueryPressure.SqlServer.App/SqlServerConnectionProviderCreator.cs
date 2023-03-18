using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;
using QueryPressure.SqlServer.Core;

[assembly: QueryPressurePlugin]
namespace QueryPressure.SqlServer.App
{
  public class SqlServerConnectionProviderCreator : ICreator<IConnectionProvider>
  {
    public string Type => "sqlserver";

    public IConnectionProvider Create(ArgumentsSection argumentsSection)
    {
      var connectionString = argumentsSection.ExtractStringArgumentOrThrow("connectionString");
      return new SqlServerConnectionProvider(connectionString);
    }
  }
}
