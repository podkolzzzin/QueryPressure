using System.Globalization;
using Perfolizer.Horology;
using QueryPressure.App.Console;
using QueryPressure.App.Interfaces;

namespace QueryPressure.Metrics.App.Formatters;

public class TimeIntervalConsoleMetricFormatter : DefaultConsoleMetricFormatter
{
  private readonly CultureInfo _cultureInfo;

  public TimeIntervalConsoleMetricFormatter(ConsoleOptions consoleOptions, IResourceManager resourceManager) : base(consoleOptions, resourceManager)
  {
    _cultureInfo = consoleOptions.CultureInfo;
  }

  public override uint Priority => 1;

  public override bool CanFormat(string metricName, object metricValue)
  {
    return metricValue is TimeInterval;
  }

  public override string GetValue(string metricName, object metricValue)
  {
    return ((TimeInterval) metricValue).ToString(_cultureInfo);
  }
}
