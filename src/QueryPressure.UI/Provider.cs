using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.UI;

public class Provider
{
  private readonly ICreator<IConnectionProvider> _creator;
  private readonly IExecutionResultStore _store;
  private readonly ILiveMetricProvider[] _liveMetricProviders;
  private readonly IScenarioBuilder _builder;
  public Provider(
    ICreator<IConnectionProvider> creator,
    IExecutionResultStore store,
    ILiveMetricProvider[] liveMetricProviders,
    IScenarioBuilder builder)
  {
    _creator = creator;
    _store = store;
    _liveMetricProviders = liveMetricProviders;
    _builder = builder;
  }

  public async Task<IServerInfo> TestConnectionAsync(string connectionString)
  {
    var provider = _creator.Create(new ArgumentsSection()
    {
      Arguments = new Dictionary<string, string>()
      {
        ["connectionString"] = connectionString // TODO: put constant connectionString somewhere
      }
    });
    return await provider.GetServerInfoAsync(default);
  }
  public async Task<Guid> StartExecutionAsync(ExecutionRequest request)
  {
    var appArgs = new ApplicationArguments()
    {
      ["connection"] = new()
      {
        Type = request.Provider,
        Arguments = new()
        {
          ["connectionString"] = request.ConnectionString,
        }
      },
      ["profile"] = new () {
        Type = request.Profile.Type,
        Arguments = request.Profile.Arguments.ToDictionary(x => x.Name, x => x.Value),
      },
      ["limit"] = new() {
        Type = request.Limit.Type,
        Arguments = request.Limit.Arguments.ToDictionary(x => x.Name, x => x.Value),
      },
      ["script"] = new () {
        Type = "text",
        Arguments = new () {
          ["text"] = request.Script
        }
      }
    };
    var executor = await _builder.BuildAsync(appArgs, _store, _liveMetricProviders, default);

    return Store(executor.ExecuteAsync(default));//TODO: put somewhere 
  }
  private Guid Store(Task executeAsync)
  {
    return Guid.NewGuid(); // TODO !!!!!
  }
}
