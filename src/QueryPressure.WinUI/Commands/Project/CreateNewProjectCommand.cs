using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Project;

namespace QueryPressure.WinUI.Commands.Project;

public class CreateNewProjectCommand : CommandBase
{
  private readonly IProjectService _projectService;
  private readonly CloseProjectCommand _closeProjectCommand;

  public CreateNewProjectCommand(IProjectService projectService, CloseProjectCommand closeProjectCommand)
  {
    _projectService = projectService;
    _closeProjectCommand = closeProjectCommand;
  }

  public override bool CanExecute(object? parameter) => true;

  protected override void ExecuteInternal(object? parameter)
  {
    if (_projectService.Project is not null)
    {
      _closeProjectCommand.Execute(null);
    }

    if (_projectService.Project is null)  // TBD TODO check if _closeProjectCommand closed the project
    {
      _projectService.CreateNew();
    }
  }
}
