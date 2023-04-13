using System.Globalization;
using System.Text;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.App.Console;

public class ConsoleMetricsVisualizer : IMetricsVisualizer
{
  private readonly ConsoleOptions _consoleOptions;
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

  public ConsoleMetricsVisualizer(ConsoleOptions consoleOptions, IConsoleMetricFormatterProvider formatterProvider)
  {
    _consoleOptions = consoleOptions;
    _formatterProvider = formatterProvider;
  }

  public Task<IVisualization> VisualizeAsync(IEnumerable<IMetric> metrics, CancellationToken cancellationToken)
  {
    var stringBuilder = new StringBuilder();
    foreach (var metric in metrics)
    {
      stringBuilder.AppendLine(new string(_consoleOptions.RowSeparatorChar, _consoleOptions.WidthInChars));
      var formattedString = _formatterProvider.Get(metric.Name).Format(metric.Name, metric.Value, CultureInfo.CurrentUICulture);
      stringBuilder.AppendLine(formattedString);
    }
    stringBuilder.AppendLine(new string(_consoleOptions.RowSeparatorChar, _consoleOptions.WidthInChars));
    return Task.FromResult<IVisualization>(new ConsoleVisualization(stringBuilder.ToString()));
  }
}
