using Autofac.Features.AttributeFilters;

namespace QueryPressure.App.Console;

public interface IConsoleMetricFormatterProvider
{
  IConsoleMetricFormatter Get(string metricName, object metricValue);
}

public class ConsoleMetricFormatterProvider : IConsoleMetricFormatterProvider
{
  private readonly IReadOnlyList<IConsoleMetricFormatter> _customMetricFormatters;
  private readonly IConsoleMetricFormatter _defaultFormatter;

  public ConsoleMetricFormatterProvider([KeyFilter("default")] IConsoleMetricFormatter defaultFormatter, IEnumerable<IConsoleMetricFormatter> customMetricFormatters)
  {
    _customMetricFormatters = customMetricFormatters.OrderByDescending(x => x.Priority).ToList();
    _defaultFormatter = defaultFormatter;
  }

  public IConsoleMetricFormatter Get(string metricName, object metricValue)
  {
    var customFormatter = _customMetricFormatters
      .FirstOrDefault(x => x.CanFormat(metricName, metricValue));

    return customFormatter ?? _defaultFormatter;
  }
}
