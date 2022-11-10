using MySql.Data.MySqlClient;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.ScriptSources;

namespace QueryPressure.MySql.Core;

public class MySqlExecutor : IExecutable
{
  private readonly IConnectionsHolder<MySqlConnection> _connections;
  private readonly TextScript _script;
  
  public MySqlExecutor(IScript script, IConnectionsHolder<MySqlConnection> connections)
  {
    if (script is not TextScript textScript)
      throw new ApplicationException("The only supported script type is TextScript");
    _connections = connections;
    _script = textScript;
  }
  public void Dispose()
  {
    _connections.Dispose();
  }
  public async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    using var holder = _connections.UseConnection();
    await using var cmd = holder.Connection.CreateCommand();
    cmd.CommandText = _script.Text;
    await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
    await reader.ReadAsync(cancellationToken);
  }
}