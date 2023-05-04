using System.IO;

namespace QueryPressure.WinUI.Models;

public class ProjectModel : IModel
{
  public ProjectModel() : this(Guid.Empty, string.Empty)
  {
  }

  public ProjectModel(Guid id, string name)
  {
    Id = id;
    Name = name;
    Profiles = new List<ProfileModel>();
  }

  public Guid Id { get; set; }
  public string Name { get; set; }

  public FileInfo? Path { get; set; }

  public List<ProfileModel> Profiles { get; }
}
