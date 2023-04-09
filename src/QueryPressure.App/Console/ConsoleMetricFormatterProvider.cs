namespace QueryPressure.App.Console;

public interface IConsoleMetricFormatterProvider
{
  IConsoleMetricFormatter Get(string metricName);
}

public class ConsoleMetricFormatterProvider : IConsoleMetricFormatterProvider
{
  private readonly IReadOnlyList<IConsoleMetricFormatter> _customMetricFormatters;
  private readonly IConsoleMetricFormatter _defaultFormatter;

  public ConsoleMetricFormatterProvider(IReadOnlyList<IConsoleMetricFormatter> customMetricFormatters, IConsoleMetricFormatter defaultFormatter)
  {
    _customMetricFormatters = customMetricFormatters;
    _defaultFormatter = defaultFormatter;
  }

  public IConsoleMetricFormatter Get(string metricName)
  {
    var customFormatter = _customMetricFormatters.FirstOrDefault(x =>
      x.SupportedMetricNames.Contains(metricName));

    return customFormatter ?? _defaultFormatter;
  }
}
