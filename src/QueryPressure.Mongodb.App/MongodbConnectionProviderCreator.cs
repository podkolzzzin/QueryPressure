using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;
using QueryPressure.Mongodb.Core;

[assembly: QueryPressurePlugin]

namespace QueryPressure.Mongodb.App;

public class MongodbConnectionProviderCreator : ICreator<IConnectionProvider>
{
  public string Type => "mongodb";

  public IConnectionProvider Create(ArgumentsSection argumentsSection)
  {
    var connectionString = argumentsSection.ExtractStringArgumentOrThrow("connectionString");
    var collectionName = argumentsSection.ExtractStringArgumentOrThrow("collectionName");
    return new MongodbConnectionProvider(connectionString, collectionName);
  }
}