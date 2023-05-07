using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

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

  [JsonIgnore]
  public FileInfo? Path { get; set; }

  public Version? ModelVersion => Assembly.GetExecutingAssembly().GetName().Version;

  public Guid Id { get; set; }

  public string Name { get; set; }

  public List<ProfileModel> Profiles { get; set; }
}
