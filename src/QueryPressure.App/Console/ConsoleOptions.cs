using System.Globalization;

namespace QueryPressure.App.Console;

public record ConsoleOptions(CultureInfo CultureInfo, char RowSeparatorChar = '-');
