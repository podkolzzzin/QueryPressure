namespace QueryPressure.WinUI.Models;

public class ProfileModel : IModel
{
  public Guid Id { get; set; }

  public string Name { get; set; }

  public List<ExecutionModel> Executions { get; }

  public bool IsReadOnly => !Executions.Any();
}
