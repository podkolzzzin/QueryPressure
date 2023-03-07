using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;
using QueryPressure.MongoDB.Core;

[assembly: QueryPressurePlugin]

namespace QueryPressure.MongoDB.App;

public class MongoDBConnectionProviderCreator : ICreator<IConnectionProvider>
{
  public string Type => "mongodb";

  public IConnectionProvider Create(ArgumentsSection argumentsSection)
  {
    var connectionString = argumentsSection.ExtractStringArgumentOrThrow("connectionString");
    var collectionName = argumentsSection.ExtractStringArgumentOrThrow("collectionName");
    return new MongoDBConnectionProvider(connectionString, collectionName);
  }
}
