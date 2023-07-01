using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Project;

namespace QueryPressure.WinUI.Commands.Project;

public class SaveProjectCommand : CommandBase
{
  private readonly IProjectService _projectService;

  public SaveProjectCommand(IProjectService projectService)
  {
    _projectService = projectService;
  }

  public override bool CanExecute(object? parameter)
  {
    var parameterPath = parameter?.ToString();
    return _projectService.Project is not null && (!string.IsNullOrEmpty(parameterPath) || _projectService.Project.Path != null);
  }

  protected override void ExecuteInternal(object? parameter)
  {
    var parameterPath = parameter?.ToString();

    var path = string.IsNullOrEmpty(parameterPath) ? _projectService.Project?.Path?.FullName : parameterPath;

    if (string.IsNullOrEmpty(path))
    {

      throw new ArgumentNullException(nameof(path));
    }

    _projectService.SaveAsync(path);
  }
}
