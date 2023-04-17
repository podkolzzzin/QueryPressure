using System.Text;
using Perfolizer.Horology;
using Perfolizer.Mathematics.Histograms;
using QueryPressure.App.Console;
using QueryPressure.App.Interfaces;

namespace QueryPressure.Metrics.App.Formatters;

public class HistogramConsoleMetricFormatter : IConsoleMetricFormatter
{
  private readonly ConsoleOptions _consoleOptions;
  private readonly IDictionary<string, string> _locale;

  public HistogramConsoleMetricFormatter(ConsoleOptions consoleOptions, IResourceManager resourceManager)
  {
    _consoleOptions = consoleOptions;
    _locale = resourceManager.GetResources(consoleOptions.CultureInfo.Name, ResourceFormat.Plain);
  }

  public uint Priority => 1;

  public bool CanFormat(string metricName, object metricValue)
  {
    return metricValue is Histogram;
  }

  public string Format(string metricName, object metricValue)
  {
    var value = (Histogram)metricValue;

    if (!_locale.TryGetValue($"metrics.{metricName}.title", out var metricDisplayName))
    {
      metricDisplayName = metricName;
    }

    var sb = new StringBuilder();
    var header = $"-------------------- {metricDisplayName} --------------------"
      .Replace('-', _consoleOptions.RowSeparatorChar);
    sb.AppendLine(header);
    sb.AppendLine(value.ToString(x =>
    {
      var timeInterval = TimeInterval.FromNanoseconds(x);
      return timeInterval.ToString(_consoleOptions.CultureInfo);
    }));
    sb.Append(string.Empty.PadRight(header.Length, _consoleOptions.RowSeparatorChar));

    return sb.ToString();
  }
}
