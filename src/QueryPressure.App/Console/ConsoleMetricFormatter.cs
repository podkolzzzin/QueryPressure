namespace QueryPressure.App.Console;

public interface IConsoleMetricFormatter
{
  HashSet<string> SupportedMetricNames { get; }

  string Format(string metricName, object metricValue, IFormatProvider formatProvider);
}

public class DefaultConsoleMetricFormatter : IConsoleMetricFormatter
{
  private readonly ConsoleOptions _consoleOptions;
  private int _padCharsFirstColumn;
  private int _padCharsSecondColumn;

  public DefaultConsoleMetricFormatter(ConsoleOptions consoleOptions)
  {
    _consoleOptions = consoleOptions;
    SupportedMetricNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    InitConsoleTablePaddings();
  }

  private void InitConsoleTablePaddings()
  {
    var width = _consoleOptions.WidthInChars;
    var tabSize = _consoleOptions.TabSize;

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

    _padCharsSecondColumn = width - (int) leftPartSize - 1;
  }

  public HashSet<string> SupportedMetricNames { get; init; }

  public string Format(string metricName, object metricValue, IFormatProvider _) =>
    $"|\t{metricName.PadRight(_padCharsFirstColumn)}|\t{(metricValue?.ToString() ?? "NULL").PadRight(_padCharsSecondColumn)}|";
}
