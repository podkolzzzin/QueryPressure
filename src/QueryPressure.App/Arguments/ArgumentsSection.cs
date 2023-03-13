namespace QueryPressure.App.Arguments;

public class ArgumentsSection
{
  public string? Type { get; set; }
  public Dictionary<string, string>? Arguments { get; set; }
}

public class ArgumentsSectionArray
{
  public string? Type { get; set; }
  public List<ArgumentFlat>? Arguments { get; set; }
}

public class ArgumentFlat
{
  public string Type { get; set; }
  public string Name { get; set; }
  public string Value { get; set; }
}
