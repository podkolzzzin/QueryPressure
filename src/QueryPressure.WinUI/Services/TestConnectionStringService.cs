using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.WinUI.Dtos;

namespace QueryPressure.WinUI.Services
{
  public interface ITestConnectionStringService
  {
    Task<IServerInfo> TestConnectionAsync(string provider, string connectionString, CancellationToken token);
  }

  public class TestConnectionStringService : ITestConnectionStringService
  {
    private readonly Dictionary<string, ICreator<IConnectionProvider>> _connectionCreatorMapper;

    public TestConnectionStringService(ICreator<IConnectionProvider>[] creators)
    {
      _connectionCreatorMapper = creators.ToDictionary(x => x.Type);
    }

    public async Task<IServerInfo> TestConnectionAsync(string provider, string connectionString, CancellationToken token)
    {
      var connectionCreator = _connectionCreatorMapper[provider.ToLower()];
      var ars = GetArguments(connectionString);
      var connectionProvider = connectionCreator.Create(ars);
      return await connectionProvider.GetServerInfoAsync(token);
    }

    private static ArgumentsSection GetArguments(string connectionString)
      => new()
      {
        Arguments = new Dictionary<string, string>()
        {
          ["connectionString"] = connectionString
        }
      };
  }
}
