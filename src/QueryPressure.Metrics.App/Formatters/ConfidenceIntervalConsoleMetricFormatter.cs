using Perfolizer.Horology;
using Perfolizer.Mathematics.Common;
using QueryPressure.App.Console;
using QueryPressure.App.Interfaces;
using System.Globalization;

namespace QueryPressure.Metrics.App.Formatters
{
  public class ConfidenceIntervalConsoleMetricFormatter : DefaultConsoleMetricFormatter
  {
    private readonly CultureInfo _cultureInfo;

    public ConfidenceIntervalConsoleMetricFormatter(ConsoleOptions consoleOptions, IResourceManager resourceManager) : base(consoleOptions, resourceManager)
    {
      _cultureInfo = consoleOptions.CultureInfo;
    }

    public override uint Priority => 1;
    public override bool CanFormat(string metricName, object metricValue)
    {
      return metricValue is ConfidenceInterval;
    }

    public override string GetValue(string metricName, object metricValue)
    {
      return ((ConfidenceInterval)metricValue).ToString(x => TimeInterval.FromNanoseconds(x).ToString(_cultureInfo));
    }
  }
}
