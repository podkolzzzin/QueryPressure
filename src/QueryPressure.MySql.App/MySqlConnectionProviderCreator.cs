using QueryPressure.Core;
using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.MySql.Core;

[assembly: QueryPressurePlugin]
namespace QueryPressure.MySql.App
{
  public class MySqlConnectionProviderCreator : ICreator<IConnectionProvider>
  {
    public string Type => "mysql";
    public IConnectionProvider Create(ArgumentsSection argumentsSection)
    {
      var connectionString = argumentsSection.ExtractStringArgumentOrThrow("connectionString");
      return new MySqlDbConnectionProvider(connectionString);
    }
  }
}
