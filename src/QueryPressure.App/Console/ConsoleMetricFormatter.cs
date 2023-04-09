namespace QueryPressure.App.Console;

public interface IConsoleMetricFormatter
{
  HashSet<string> SupportedMetricNames { get; }

  string Format(string metricName, object metricValue, IFormatProvider formatProvider);
}

public class DefaultConsoleMetricFormatter : IConsoleMetricFormatter
{
  public DefaultConsoleMetricFormatter()
  {
    SupportedMetricNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
  }

  public HashSet<string> SupportedMetricNames { get; init; }

  public string Format(string metricName, object metricValue, IFormatProvider _) => $"|\t{metricName,-31}|\t{metricValue,-31}|";
}
