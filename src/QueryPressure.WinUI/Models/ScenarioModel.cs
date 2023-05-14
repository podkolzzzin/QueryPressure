using System.Text.Json.Serialization;
using QueryPressure.App.Arguments;

namespace QueryPressure.WinUI.Models;

public class ScenarioModel : IModel
{
  public ScenarioModel()
  {
    Name = "Unknown";
    Executions = new List<ExecutionModel>();
  }

  public Guid Id { get; set; }

  public string Name { get; set; }

  public string? Provider { get; set; }

  public string? ConnectionString { get; set; }

  public string? Script { get; set; }

  public FlatArgumentsSection? Profile { get; set; }

  public FlatArgumentsSection? Limit { get; set; }

  public List<ExecutionModel> Executions { get; }

  [JsonIgnore]
  public bool IsReadOnly => Executions.Any();
}
