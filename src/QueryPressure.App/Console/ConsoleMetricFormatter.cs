using QueryPressure.App.Interfaces;

namespace QueryPressure.App.Console;

public interface IConsoleMetricFormatter
{
  uint Priority { get; }

  bool CanFormat(string metricName, object metricValue);

  string Format(string metricName, object metricValue);
}

public interface IConsoleMetricRowFormatter : IConsoleMetricFormatter
{
  string GetHeader(string metricName, object metricValue);
  string GetValue(string metricName, object metricValue);
}

public class DefaultConsoleMetricFormatter : IConsoleMetricRowFormatter
{
  private readonly IDictionary<string, string> _locale;

  public DefaultConsoleMetricFormatter(ConsoleOptions consoleOptions, IResourceManager resourceManager)
  {
    _locale = resourceManager.GetResources(consoleOptions.CultureInfo.Name, ResourceFormat.Plain);
  }

  public virtual uint Priority => 0;

  public virtual bool CanFormat(string metricName, object metricValue) => true;

  public string Format(string metricName, object metricValue)
  {
    return $"{GetHeader(metricName, metricValue)} = {GetValue(metricName, metricValue)}";
  }

  public virtual string GetHeader(string metricName, object metricValue)
  {
    if (!_locale.TryGetValue($"metrics.{metricName}.title", out var metricDisplayName))
    {
      metricDisplayName = metricName;
    }

    return metricDisplayName;
  }

  public virtual string GetValue(string metricName, object metricValue)
  {
    return metricValue?.ToString() ?? "NULL";
  }
}
