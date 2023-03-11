namespace QueryPressure.App.Arguments;

public class ArgumentsSection
{
  public string? Type { get; set; }
  public Dictionary<string, string>? Arguments { get; set; }
}

public class ArgumentsSection1
{
  public string Type { get; set; }
  public string Name { get; set; }
  public string Value { get; set; }
}

public class ArgumentsSectionArray
{
  public string? Type { get; set; }
  public List<ArgumentsSection1>? Arguments { get; set; }
}
