using System.Windows;
using QueryPressure.WinUI.Commands.Project;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Project;

namespace QueryPressure.WinUI.Commands.App;

public class ExitCommand : CommandBase
{
  private readonly IProjectService _projectService;
  private readonly CloseProjectCommand _closeProjectCommand;

  public ExitCommand(IProjectService projectService, CloseProjectCommand closeProjectCommand)
  {
    _projectService = projectService;
    _closeProjectCommand = closeProjectCommand;
  }

  public override bool CanExecute(object? parameter) => true;

  public override void Execute(object? parameter)
  {
    if (_projectService.Project is not null)
    {
      _closeProjectCommand.Execute(null);
    }

    Application.Current.Shutdown();
  }
}
