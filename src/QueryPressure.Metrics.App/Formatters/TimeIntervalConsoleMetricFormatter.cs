using System.Globalization;
using Autofac.Features.AttributeFilters;
using Perfolizer.Horology;
using QueryPressure.App.Console;

namespace QueryPressure.Metrics.App.Formatters;

public class TimeIntervalConsoleMetricFormatter : IConsoleMetricFormatter
{
  private readonly CultureInfo _cultureInfo;
  private readonly IConsoleMetricFormatter _defaultFormatter;

  public TimeIntervalConsoleMetricFormatter(CultureInfo cultureInfo, [KeyFilter("default")] IConsoleMetricFormatter defaultFormatter)
  {
    _cultureInfo = cultureInfo;
    _defaultFormatter = defaultFormatter;
  }

  public uint Priority => 1;

  public bool CanFormat(string metricName, object metricValue)
  {
    return metricValue is TimeInterval;
  }

  public string Format(string metricName, object metricValue)
  {
    return _defaultFormatter.Format(metricName, ((TimeInterval)metricValue).ToString(_cultureInfo));
  }
}
