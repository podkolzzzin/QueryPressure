using QueryPressure.Core.Interfaces;
using QueryPressure.UI.Hubs;

namespace QueryPressure.UI;

public class DashboardVisualizer : IMetricsVisualizer
{
  private record DashboardVisualization : IVisualization;

  private readonly IHubService<DashboardHub> _hubService;
  public const string Key = "Dashboard";

  public DashboardVisualizer(IHubService<DashboardHub> hubService)
  {
    _hubService = hubService;
  }
  
  public async Task<IVisualization> VisualizeAsync(IEnumerable<IMetric> metrics, CancellationToken cancellationToken)
  {
    await _hubService.SendMessageToAllAsync("metrics", metrics);

    return new DashboardVisualization();
  }
}
