using System.Data;
using System.Data.Common;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.ScriptSources;

namespace QueryPressure.Core;

public class DbExecutableBase<TConnection> : IExecutable where TConnection : IDbConnection
{
  private readonly IConnectionPool<TConnection> _connections;
  private readonly TextScript _script;
  
  public DbExecutableBase(IScript script, IConnectionPool<TConnection> connections)
  {
    if (script is not TextScript textScript)
      throw new ApplicationException("The only supported script type is TextScript");
    _connections = connections;
    _script = textScript;
  }


  public void Dispose() => _connections.Dispose();
  
  public async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    using var holder = _connections.UseConnection();
    await using var cmd = (DbCommand)holder.Connection.CreateCommand();
    cmd.CommandText = _script.Text;
    await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
    await reader.ReadAsync(cancellationToken);
  }
}
