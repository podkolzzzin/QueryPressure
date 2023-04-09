using System.Text;
using Perfolizer.Mathematics.Histograms;
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
    const char padNameChar = '-';

    if (metricValue is not Histogram value)
    {
      throw new ArgumentException($"The parameter '{nameof(metricValue)}' should be '{nameof(Histogram)}' type");
    }

    var sb = new StringBuilder();
    sb.Append(new string(padNameChar, 40 - metricName.Length / 2));
    sb.AppendLine(metricName.PadRight(40, padNameChar));
    sb.Append(value.ToString(x =>
    {
      var timeSpan = TimeSpan.FromMilliseconds(x);
      return timeSpan.ToString("g", formatProvider);
    }));

    return sb.ToString();
  }
}
