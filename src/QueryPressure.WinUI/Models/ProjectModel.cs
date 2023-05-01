namespace QueryPressure.WinUI.Models
{
  public class ProjectModel : IModel
  {
    public ProjectModel()
    {
      Name = string.Empty;
    }

    public ProjectModel(Guid id, string name)
    {
      Id = id;
      Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
  }


}
