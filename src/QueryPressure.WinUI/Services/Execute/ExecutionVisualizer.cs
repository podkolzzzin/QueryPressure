using QueryPressure.Core.Interfaces;

namespace QueryPressure.WinUI.Services.Execute;

public class ExecutionVisualizer : IMetricsVisualizer
{
  public const string Key = "Execution";

  public Task<IVisualization> VisualizeAsync(IEnumerable<IMetric> metrics, CancellationToken cancellationToken)
  {
    return Task.FromResult<IVisualization>(new ExecutionVisualization(metrics.ToArray()));
  }
}
