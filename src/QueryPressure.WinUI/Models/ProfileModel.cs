using System.Text.Json.Serialization;

namespace QueryPressure.WinUI.Models;

public class ProfileModel : IModel
{
  public Guid Id { get; set; }

  public string Name { get; set; }

  public List<ExecutionModel> Executions { get; }

  [JsonIgnore]
  public bool IsReadOnly => !Executions.Any();
}
