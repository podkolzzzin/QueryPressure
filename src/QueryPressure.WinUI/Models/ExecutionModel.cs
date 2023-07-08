namespace QueryPressure.WinUI.Models;

public class ExecutionModel : IModel
{
  public ExecutionModel(Guid guid)
  {
    Id = guid;
  }

  public Guid Id { get; set; }
}
