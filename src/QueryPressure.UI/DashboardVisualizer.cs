using QueryPressure.Core.Interfaces;

namespace QueryPressure.UI;

public class DashboardVisualizer : IMetricsVisualizer
{
  private record DashboardVisualization(IMetric[] Metrics) : IVisualization;
  public const string Key = "Dashboard";
  public async Task<IVisualization> VisualizeAsync(IEnumerable<IMetric> metrics, CancellationToken cancellationToken)
  {
    return new DashboardVisualization(metrics.ToArray());
  }
}
