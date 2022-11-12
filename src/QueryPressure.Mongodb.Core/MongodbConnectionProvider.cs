using MongoDB.Driver;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Requirements;

namespace QueryPressure.Mongodb.Core;

public class MongodbConnectionProvider : IConnectionProvider
{
  private readonly string _connectionString;
  private readonly string _collectionName;

  public MongodbConnectionProvider(string connectionString, string collectionName)
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
    return new MongodbExecutable(script, collection);
  }
}