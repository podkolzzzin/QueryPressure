using System.Globalization;
using System.Text;
using QueryPressure.App.Console;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.App;

public class ConsoleMetricsVisualizer : IMetricsVisualizer
{
  public const char ConsoleRowSeparatorChar = '-';
  public const int ConsoleCharWidth = 60;
  public const int TabSize = 4;

  private readonly IConsoleMetricFormatterProvider _formatterProvider;

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

  public ConsoleMetricsVisualizer(IConsoleMetricFormatterProvider formatterProvider)
  {
    _formatterProvider = formatterProvider;
  }

  public Task<IVisualization> VisualizeAsync(IEnumerable<IMetric> metrics, CancellationToken cancellationToken)
  {
    var stringBuilder = new StringBuilder();
    foreach (var metric in metrics)
    {
      stringBuilder.AppendLine(new string(ConsoleRowSeparatorChar, ConsoleCharWidth));
      var formattedString = _formatterProvider.Get(metric.Name).Format(metric.Name, metric.Value, CultureInfo.CurrentUICulture);
      stringBuilder.AppendLine(formattedString);
    }
    stringBuilder.AppendLine(new string(ConsoleRowSeparatorChar, ConsoleCharWidth));
    return Task.FromResult<IVisualization>(new ConsoleVisualization(stringBuilder.ToString()));
  }
}
