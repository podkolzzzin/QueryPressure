using QueryPressure.Core.Interfaces;

namespace QueryPressure.WinUI.Services.Core;

public interface IProvider
{
  Task<IServerInfo> TestConnectionAsync(string connectionString);
}
