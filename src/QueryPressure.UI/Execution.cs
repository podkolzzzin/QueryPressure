using Autofac.Features.AttributeFilters;
using MongoDB.Bson;
using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.UI.Hubs;

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
    [KeyFilter(DashboardVisualizer.Key)]IMetricsVisualizer visualizer,
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
      NotifyAsync(liveVisualization);
    }
  }
  
  private void NotifyAsync(IVisualization liveVisualization)
  {
    Console.WriteLine(liveVisualization.ToJson());
    _hubService.SendMessageToAllAsync("live-metrics", liveVisualization);
  }
}
