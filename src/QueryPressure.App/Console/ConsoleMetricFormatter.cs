namespace QueryPressure.App.Console;

public interface IConsoleMetricFormatter
{
  HashSet<string> SupportedMetricNames { get; }

  string Format(string metricName, object metricValue, IFormatProvider formatProvider);
}

public class DefaultConsoleMetricFormatter : IConsoleMetricFormatter
{
  private static int _padCharsFirstColumn;
  private static int _padCharsSecondColumn;

  public DefaultConsoleMetricFormatter()
  {
    SupportedMetricNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    InitConsoleTablePaddings();
  }

  private static void InitConsoleTablePaddings()
  {
    const int width = ConsoleMetricsVisualizer.ConsoleCharWidth;
    const int tabSize = ConsoleMetricsVisualizer.TabSize;

    _padCharsFirstColumn = width / 2 - tabSize * 2;

    var leftPartSize = (double) width / 2;

    if (leftPartSize % tabSize == 0)
    {
      leftPartSize += 2 * tabSize;
    }
    else
    {
      leftPartSize = Math.Ceiling(leftPartSize / tabSize) * tabSize;
    }

    _padCharsSecondColumn = width - (int)leftPartSize - 1;
  }

  public HashSet<string> SupportedMetricNames { get; init; }

  public string Format(string metricName, object metricValue, IFormatProvider _) =>
    FormatRow(metricName, metricValue?.ToString() ?? string.Empty);

  public static string FormatRow(string metricName, string metricValue)
  {
    return $"|\t{metricName.PadRight(_padCharsFirstColumn)}|\t{metricValue.PadRight(_padCharsSecondColumn)}|";
  }
    
}   
