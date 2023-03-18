using MongoDB.Driver;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.ScriptSources;

namespace QueryPressure.MongoDB.Core;

public class MongoDBExecutable : IExecutable
{
  private readonly IMongoCollection<object> _collection;
  private readonly TextScript _script;

  public MongoDBExecutable(IScript script, IMongoCollection<object> collection)
  {
    if (script is not TextScript textScript)
      throw new ApplicationException("The only supported script type is TextScript");
    _collection = collection;
    _script = textScript;
  }

  public async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    var filter = _script.Text;
    using var cursor = await _collection.FindAsync(filter, cancellationToken: cancellationToken);
    await cursor.MoveNextAsync(cancellationToken);
  }

  public void Dispose() { }
}
