using System.Text;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.App;

public class ConsoleMetricsVisualizer : IMetricsVisualizer
{
  private class ConsoleVisualization : IVisualization
  {
    private readonly string _view;
    public ConsoleVisualization(string view)
    {
      _view = view;

    }

    public override string ToString()
    {
      return _view;
    }
  }

  public Task<IVisualization> VisualizeAsync(IEnumerable<IMetric> metrics, CancellationToken cancellationToken)
  {
    var stringBuilder = new StringBuilder();
    foreach (var metric in metrics)
    {
      stringBuilder.AppendLine(metric.Name + "=" + metric.Value);
    }
    return Task.FromResult<IVisualization>(new ConsoleVisualization(stringBuilder.ToString()));
  }
}
