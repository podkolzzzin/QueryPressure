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
      return $"|\t{metricName,-31}|\t{value.ToString("g", formatProvider),-31}|";
    }

    throw new ArgumentException($"The parameter '{nameof(metricValue)}' should be '{nameof(TimeSpan)}' type");
  }
}
