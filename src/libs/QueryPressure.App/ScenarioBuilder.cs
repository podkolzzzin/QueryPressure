using QueryPressure.App.Arguments;
using QueryPressure.App.Factories;
using QueryPressure.App.Interfaces;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Requirements;

namespace QueryPressure.App;

public class ScenarioBuilder : IScenarioBuilder
{
  private readonly ISettingsFactory<IProfile> _profilesFactory;
  private readonly ISettingsFactory<ILimit> _limitsFactory;
  private readonly ISettingsFactory<IConnectionProvider> _connectionProviderFactory;
  private readonly ISettingsFactory<IScriptSource> _scriptSourceFactory;

  public ScenarioBuilder(
    ISettingsFactory<IProfile> profilesFactory,
    ISettingsFactory<ILimit> limitsFactory,
    ISettingsFactory<IConnectionProvider> connectionProviderFactory,
    ISettingsFactory<IScriptSource> scriptSourceFactory)
  {
    _profilesFactory = profilesFactory;
    _limitsFactory = limitsFactory;
    _connectionProviderFactory = connectionProviderFactory;
    _scriptSourceFactory = scriptSourceFactory;

  }

  public async Task<QueryExecutor> BuildAsync(ApplicationArguments arguments, IExecutionResultStore store, IEnumerable<IExecutionHook> otherHooks, CancellationToken cancellationToken)
  {
    var profile = _profilesFactory.Create(arguments);
    var limit = _limitsFactory.Create(arguments);
    var connectionProvider = _connectionProviderFactory.Create(arguments);
    var scriptSource = _scriptSourceFactory.Create(arguments);

    var requirements = profile.Requirements
      .Concat(limit.Requirements)
      .Concat(connectionProvider.Requirements)
      .Concat(scriptSource.Requirements);

    var requirementsService = new RequirementService(requirements);

    var executor = await connectionProvider.CreateExecutorAsync(scriptSource, requirementsService.GetRequirement<ConnectionRequirement>(), cancellationToken);
    return new QueryExecutor(executor, profile, limit, store, otherHooks);
  }
}
