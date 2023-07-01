using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.Services.Project;

public interface IProjectService
{
  ProjectModel? Project { get; }

  Task OpenAsync(string path, CancellationToken token = default);

  Task SaveAsync(string path, CancellationToken token = default);

  Task SaveAsync(CancellationToken token = default);

  void CreateNew();

  void Close();
}
