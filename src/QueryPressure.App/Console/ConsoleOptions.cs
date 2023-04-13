using System.Globalization;

namespace QueryPressure.App.Console;

public record ConsoleOptions(CultureInfo CultureInfo, char RowSeparatorChar = '-', int WidthInChars = 60, int TabSize = 4);
