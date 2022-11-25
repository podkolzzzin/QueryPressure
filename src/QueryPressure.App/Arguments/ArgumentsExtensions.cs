namespace QueryPressure.App.Arguments;

public static class ArgumentsExtensions
{
  public static string ExtractStringArgumentOrThrow(this ArgumentsSection argumentsSection, string name)
  {
    if (argumentsSection.Arguments == null || !argumentsSection.Arguments.TryGetValue(name, out var val))
      throw new ArgumentException($"There is no argument with named '{name}' in {argumentsSection.Type}");
    return val;
  }

  public static int ExtractIntArgumentOrThrow(this ArgumentsSection argumentsSection, string name)
  {
    var val = ExtractStringArgumentOrThrow(argumentsSection, name);
    if (!int.TryParse(val, out var result))
      throw new ArgumentException(
        $"The value presented as an argument named '{name}' is not a valid integer. The value is '{val}'");
    return result;
  }

  public static TimeSpan ExtractTimeSpanArgumentOrThrow(this ArgumentsSection argumentsSection, string name)
  {
    var val = ExtractStringArgumentOrThrow(argumentsSection, name);
    if (!TimeSpan.TryParse(val, out var result))
      throw new ArgumentException(
        $"The value presented as an argument named '{name}' is not a valid TimeSpan. The value is '{val}'");
    return result;
  }
}
