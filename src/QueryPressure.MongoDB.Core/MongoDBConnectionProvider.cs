using MongoDB.Driver;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Requirements;

namespace QueryPressure.MongoDB.Core;

public class MongoDBConnectionProvider : IConnectionProvider
{
  private readonly string _connectionString;
  private readonly string _collectionName;

  public MongoDBConnectionProvider(string connectionString, string collectionName)
  {
    _connectionString = connectionString;
    _collectionName = collectionName;
  }

  public async Task<IExecutable> CreateExecutorAsync(IScriptSource scriptSource, ConnectionRequirement connectionRequirement,
      CancellationToken cancellationToken)
  {
    var script = await scriptSource.GetScriptAsync(cancellationToken);

    var connection = new MongoClient(_connectionString);
    var databaseName = MongoUrl.Create(_connectionString).DatabaseName;

    var db = connection.GetDatabase(databaseName);
    var collection = db.GetCollection<object>(_collectionName);
    return new MongoDBExecutable(script, collection);
  }
}
