using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.Services.Project;

public interface IProjectService
{
  ProjectModel? Project { get; }

  Task OpenAsync(string path, CancellationToken token);

  Task SaveAsync(string path, CancellationToken token);

  Task SaveAsync(CancellationToken token);

  void CreateNew();

  void Close();
}
