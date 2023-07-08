using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.Services.Core;

public class Provider : IProvider
{
  private readonly ICreator<IConnectionProvider> _creator;
  private readonly IExecutionResultStore _store;
  private readonly ILiveMetricProvider[] _liveMetricProviders;
  private readonly IScenarioBuilder _builder;
  private readonly ISubscriptionManager _subscriptionManager;

  public Provider(
    ICreator<IConnectionProvider> creator,
    IExecutionResultStore store,
    ILiveMetricProvider[] liveMetricProviders,
    IScenarioBuilder builder,
    ISubscriptionManager subscriptionManager)
  {
    _creator = creator;
    _store = store;
    _liveMetricProviders = liveMetricProviders;
    _builder = builder;
    _subscriptionManager = subscriptionManager;
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

  public async Task<Guid> StartExecutionAsync(ScenarioModel scenario)
  {
    var appArgs = new ApplicationArguments()
    {
      ["connection"] = new()
      {
        Type = scenario.Provider,
        Arguments = new()
        {
          ["connectionString"] = scenario.ConnectionString!,
        }
      },
      ["profile"] = new()
      {
        Type = scenario.Profile.Type,
        Arguments = scenario.Profile.Arguments!.ToDictionary(x => x.Name, x => x.Value),
      },
      ["limit"] = new()
      {
        Type = scenario.Limit.Type,
        Arguments = scenario.Limit.Arguments!.ToDictionary(x => x.Name, x => x.Value),
      },
      ["script"] = new()
      {
        Type = "text",
        Arguments = new()
        {
          ["text"] = scenario!.Script!
        }
      }
    };
    var executor = await _builder.BuildAsync(appArgs, _store, _liveMetricProviders, default);

    var executionId = Guid.NewGuid();
    var task = executor.ExecuteAsync(default);

    throw new NotImplementedException("Add Task to ExecutionWatcher");

    var executionModel = new ExecutionModel(executionId);
    scenario.Executions.Add(executionModel);
    _subscriptionManager.Notify(this, ModelAction.ChildrenChanged, scenario);

    return executionModel.Id;
  }
}
