using System.Text;
using Perfolizer.Mathematics.Histograms;
using QueryPressure.App.Console;

namespace QueryPressure.Metrics.App.Formatters;

public class HistogramConsoleMetricFormatter : IConsoleMetricFormatter
{
  private readonly ConsoleOptions _consoleOptions;

  public HistogramConsoleMetricFormatter(ConsoleOptions consoleOptions)
  {
    _consoleOptions = consoleOptions;
    SupportedMetricNames = new HashSet<string> { "Histogram" };
  }

  public HashSet<string> SupportedMetricNames { get; }

  public string Format(string metricName, object metricValue, IFormatProvider formatProvider)
  {
    if (metricValue is not Histogram value)
    {
      throw new ArgumentException($"The parameter '{nameof(metricValue)}' should be '{nameof(Histogram)}' type");
    }

    var separator = _consoleOptions.RowSeparatorChar;
    var width = _consoleOptions.WidthInChars;

    var sb = new StringBuilder();
    sb.Append(new string(separator, width / 2 - metricName.Length / 2));
    sb.AppendLine(metricName.PadRight(width / 2 + metricName.Length / 2, separator));
    sb.Append(value.ToString(x =>
    {
      var timeSpan = TimeSpan.FromMilliseconds(x);
      return timeSpan.ToString("g", formatProvider);
    }));

    return sb.ToString();
  }
}
