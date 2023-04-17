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

    var metricFormatters = metrics
      .Select(x => new MetricValueFormatter(x, _formatterProvider.Get(x.Name, x.Value)))
      .GroupBy(x => x.Formatter is IConsoleMetricRowFormatter)
      .ToDictionary(g => g.Key, g => g);

    if (metricFormatters.TryGetValue(true, out var rowsGroup))
    {
      var rows = rowsGroup
        .Select(x => new MetricRowValueFormatter(x.Metric, (IConsoleMetricRowFormatter)x.Formatter)).ToList();

      DrawTable(stringBuilder, rows);
    }

    if (metricFormatters.TryGetValue(false, out var otherMetrics))
    {
      foreach (var metricFormatter in otherMetrics)
      {
        var formattedString = metricFormatter.Formatter.Format(metricFormatter.Metric.Name, metricFormatter.Metric.Value);
        stringBuilder.AppendLine(formattedString);
      }
    }

    return Task.FromResult<IVisualization>(new ConsoleVisualization(stringBuilder.ToString()));
  }

  private void DrawTable(StringBuilder stringBuilder, List<MetricRowValueFormatter> metricFormatter)
  {
    var maxHeaderSize = metricFormatter.Max(x =>
      x.Formatter.GetHeader(x.Metric.Name, x.Metric.Value).Length);

    var maxValueSize = metricFormatter.Max(x =>
      x.Formatter.GetValue(x.Metric.Name, x.Metric.Value).Length);

    var totalTableSize = 7 + maxHeaderSize + maxValueSize;

    stringBuilder.AppendLine(new string(_consoleOptions.RowSeparatorChar, totalTableSize));

    foreach (var item in metricFormatter)
    {
      var headerDisplayString = item.Formatter.GetHeader(item.Metric.Name, item.Metric.Value);
      var valueDisplayString = item.Formatter.GetValue(item.Metric.Name, item.Metric.Value);
      stringBuilder.AppendLine($"| {headerDisplayString.PadRight(maxHeaderSize)} | {valueDisplayString.PadLeft(maxValueSize)} |");
    }

    stringBuilder.AppendLine(new string(_consoleOptions.RowSeparatorChar, totalTableSize));
  }

  private readonly record struct MetricValueFormatter(IMetric Metric, IConsoleMetricFormatter Formatter);
  private readonly record struct MetricRowValueFormatter(IMetric Metric, IConsoleMetricRowFormatter Formatter);
}
