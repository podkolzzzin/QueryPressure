using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Project;

namespace QueryPressure.WinUI.Commands;

public class CreateNewProjectCommand : CommandBase
{
  private readonly IProjectService _projectService;

  public CreateNewProjectCommand(IProjectService projectService)
  {
    _projectService = projectService;
  }

  public override bool CanExecute(object? parameter) => true;

  public override void Execute(object? parameter)
  {
    _projectService.CreateNew();
  }
}
