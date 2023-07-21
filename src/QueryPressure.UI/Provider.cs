using Autofac;
using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.UI;

public class Provider
{
  private readonly ICreator<IConnectionProvider> _creator;
  private readonly IScenarioBuilder _builder;
  private readonly IExecutionStore _executionStore;
  private readonly IComponentContext _container;

  public Provider(
    ICreator<IConnectionProvider> creator,
    IScenarioBuilder builder, 
    IExecutionStore executionStore, 
    IComponentContext container)
  {
    _creator = creator;
    _builder = builder;
    _executionStore = executionStore;
    _container = container;
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

  public async Task<Guid> StartExecutionAsync(ExecutionRequest request, CancellationToken cancellationToken)
  {
    var appArgs = BuildArgs(request);
    
    var store = _container.Resolve<IExecutionResultStore>();
    var liveMetricProviders = _container.Resolve<ILiveMetricProvider[]>();
    
    var executor = await _builder.BuildAsync(appArgs, store, liveMetricProviders, cancellationToken);
    
    var cts = new CancellationTokenSource();
    var execution = executor.ExecuteAsync(cts.Token);

    return await _executionStore.SaveAsync(execution, store, liveMetricProviders, cts);
  }

  private static ApplicationArguments BuildArgs(ExecutionRequest request)
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
      ["profile"] = new()
      {
        Type = request.Profile.Type,
        Arguments = request.Profile.Arguments.ToDictionary(x => x.Name, x => x.Value),
      },
      ["limit"] = new()
      {
        Type = request.Limit.Type,
        Arguments = request.Limit.Arguments.ToDictionary(x => x.Name, x => x.Value),
      },
      ["script"] = new()
      {
        Type = "text",
        Arguments = new()
        {
          ["text"] = request.Script
        }
      }
    };
    return appArgs;
  }
}
