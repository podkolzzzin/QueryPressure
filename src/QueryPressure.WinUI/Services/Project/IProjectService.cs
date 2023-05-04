using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.Services.Project;

public interface IProjectService
{
  Task OpenProjectAsync(string parameter, CancellationToken token);
  void CreateNew();
  public ProjectModel? Project { get; }
  void Close();
}
