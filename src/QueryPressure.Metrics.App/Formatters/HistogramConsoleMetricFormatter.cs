using System.Text;
using Perfolizer.Mathematics.Histograms;
using QueryPressure.App;
using QueryPressure.App.Console;

namespace QueryPressure.Metrics.App.Formatters;

public class HistogramConsoleMetricFormatter : IConsoleMetricFormatter
{
  public HistogramConsoleMetricFormatter()
  {
    SupportedMetricNames = new HashSet<string> { "Histogram" };
  }

  public HashSet<string> SupportedMetricNames { get; }

  public string Format(string metricName, object metricValue, IFormatProvider formatProvider)
  {
    if (metricValue is not Histogram value)
    {
      throw new ArgumentException($"The parameter '{nameof(metricValue)}' should be '{nameof(Histogram)}' type");
    }

    var separator = ConsoleMetricsVisualizer.ConsoleRowSeparatorChar;
    var consoleWidth = ConsoleMetricsVisualizer.ConsoleCharWidth;

    var sb = new StringBuilder();
    sb.Append(new string(separator, consoleWidth / 2 - metricName.Length / 2));
    sb.AppendLine(metricName.PadRight(consoleWidth / 2 + metricName.Length / 2, separator));
    sb.Append(value.ToString(x =>
    {
      var timeSpan = TimeSpan.FromMilliseconds(x);
      return timeSpan.ToString("g", formatProvider);
    }));

    return sb.ToString();
  }
}
