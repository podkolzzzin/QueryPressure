using Autofac.Features.AttributeFilters;
using MongoDB.Bson;
using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.UI.Hubs;
using QueryPressure.UI.Interfaces;

namespace QueryPressure.UI;

internal class Execution // TODO: interesting idea for refactoring: use MediatR to send notifications and reuse this class in CLI
{
  private readonly ApplicationArguments _args;
  private readonly IMetricsVisualizer _visualizer;
  private readonly IHubService<DashboardHub> _hubService;
  private readonly IExecutionResultStore _store;
  private readonly ILiveMetricProvider[] _liveMetricProviders;
  private readonly IScenarioBuilder _builder;

  public Execution(
    ApplicationArguments args,
    Guid id,
    [KeyFilter(DashboardVisualizer.Key)] IMetricsVisualizer visualizer,
    IHubService<DashboardHub> hubService,
    IExecutionResultStore store,
    ILiveMetricProvider[] liveMetricProviders,
    IScenarioBuilder builder)
  {
    Id = id;

    _args = args;
    _visualizer = visualizer;
    _hubService = hubService;
    _store = store;
    _liveMetricProviders = liveMetricProviders;
    _builder = builder;
  }

  public Guid Id { get; }

  public async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    var executor = await _builder.BuildAsync(_args, _store, _liveMetricProviders, default);
    var executeTask = executor.ExecuteAsync(cancellationToken);
    while (!executeTask.IsCompleted)
    {
      await Task.WhenAny(executeTask, Task.Delay(200, cancellationToken));
      var liveVisualization = await _visualizer.VisualizeAsync(_liveMetricProviders.SelectMany(x => x.GetMetrics()), cancellationToken);
      await NotifyAsync(liveVisualization);
    }
    await _hubService.SendCompletionStatusAsync(Id, executeTask.IsCompletedSuccessfully, executeTask.Exception?.Message, default);
  }

  private async Task NotifyAsync(IVisualization liveVisualization)
  {
    Console.WriteLine(liveVisualization.ToJson());
    await _hubService.SendExecutionMetricAsync(Id, liveVisualization, default);
  }
}
