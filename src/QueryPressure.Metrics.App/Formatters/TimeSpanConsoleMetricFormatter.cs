using Autofac.Features.AttributeFilters;
using QueryPressure.App.Console;

namespace QueryPressure.Metrics.App.Formatters;

public class TimeSpanConsoleMetricFormatter : IConsoleMetricFormatter
{
  private readonly IConsoleMetricFormatter _defaultFormatter;

  public TimeSpanConsoleMetricFormatter([KeyFilter("default")] IConsoleMetricFormatter defaultFormatter)
  {
    _defaultFormatter = defaultFormatter;
    SupportedMetricNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
      "q1",
      "median",
      "q3",
      "standard-deviation",
      "mean",
      "live-average"
    };
  }

  public HashSet<string> SupportedMetricNames { get; }
  public string Format(string metricName, object metricValue, IFormatProvider formatProvider)
  {
    if (metricValue is TimeSpan value)
    {
      return _defaultFormatter.Format(metricName, value.ToString("g", formatProvider), formatProvider);
    }

    throw new ArgumentException($"The parameter '{nameof(metricValue)}' should be '{nameof(TimeSpan)}' type");
  }
}
