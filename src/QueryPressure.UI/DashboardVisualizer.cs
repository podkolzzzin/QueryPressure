using QueryPressure.Core.Interfaces;

namespace QueryPressure.UI;

public class DashboardVisualizer : IMetricsVisualizer
{
  private record DashboardVisualization(IMetric[] Metrics) : IVisualization;
  public const string Key = "Dashboard";
  public Task<IVisualization> VisualizeAsync(IEnumerable<IMetric> metrics, CancellationToken cancellationToken)
  {
    return Task.FromResult<IVisualization>(new DashboardVisualization(metrics.ToArray()));
  }
}
