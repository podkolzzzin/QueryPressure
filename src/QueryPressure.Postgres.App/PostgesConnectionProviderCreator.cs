using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Postgres.Core;

namespace QueryPressure.Postgres.App;

public class PostgesConnectionProviderCreator : ICreator<IConnectionProvider>
{
  public string Type => "postgres";
  public IConnectionProvider Create(ArgumentsSection argumentsSection)
  {
    var connectionString = argumentsSection.ExtractStringArgumentOrThrow("connectionString");
    return new PostgresConnectionProvider(connectionString);
  }
}