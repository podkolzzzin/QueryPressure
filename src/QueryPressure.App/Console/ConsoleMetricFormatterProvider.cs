using Autofac.Features.AttributeFilters;

namespace QueryPressure.App.Console;

public interface IConsoleMetricFormatterProvider
{
  IConsoleMetricFormatter Get(string metricName);
}

public class ConsoleMetricFormatterProvider : IConsoleMetricFormatterProvider
{
  private readonly IReadOnlyList<IConsoleMetricFormatter> _customMetricFormatters;
  private readonly IConsoleMetricFormatter _defaultFormatter;

  public ConsoleMetricFormatterProvider([KeyFilter("default")] IConsoleMetricFormatter defaultFormatter, IReadOnlyList<IConsoleMetricFormatter> customMetricFormatters)
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
