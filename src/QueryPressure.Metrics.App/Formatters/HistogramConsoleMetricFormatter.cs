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

    var separator = _consoleOptions.RowSeparatorChar;
    var width = _consoleOptions.WidthInChars;

    var metricDisplayName = _locale[$"metrics.{metricName}.title"];

    var sb = new StringBuilder();
    sb.Append(new string(separator, width / 2 - metricName.Length / 2));
    sb.AppendLine(metricDisplayName.PadRight(width / 2 + metricName.Length / 2, separator));
    sb.Append(value.ToString(x =>
    {
      var timeInterval = TimeInterval.FromNanoseconds(x);
      return timeInterval.ToString(_consoleOptions.CultureInfo);
    }));

    return sb.ToString();
  }
}
