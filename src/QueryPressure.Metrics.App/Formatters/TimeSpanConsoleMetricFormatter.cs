using QueryPressure.App.Console;

namespace QueryPressure.Metrics.App.Formatters;

public class TimeSpanConsoleMetricFormatter : IConsoleMetricFormatter
{
  public TimeSpanConsoleMetricFormatter()
  {
    SupportedMetricNames = new HashSet<string>
    {
      "Q1",
      "Median",
      "Q3",
      "StandardDeviation",
      "Mean"
    };
  }

  public HashSet<string> SupportedMetricNames { get; }
  public string Format(string metricName, object metricValue, IFormatProvider formatProvider)
  {
    if (metricValue is TimeSpan value)
    {
      return DefaultConsoleMetricFormatter.FormatRow(metricName, value.ToString("g", formatProvider));
    }

    throw new ArgumentException($"The parameter '{nameof(metricValue)}' should be '{nameof(TimeSpan)}' type");
  }
}
