using System.Windows;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Project;

namespace QueryPressure.WinUI.Commands.Project;

public class CloseProjectCommand : CommandBase
{
  private readonly IProjectService _projectService;

  public CloseProjectCommand(IProjectService projectService)
  {
    _projectService = projectService;
  }

  public override bool CanExecute(object? parameter) => _projectService.Project is not null;

  public override void Execute(object? parameter)
  {
    var dialog = MessageBox.Show(
      "Are you sure that you want to close current project?",
      "Close Project",
      MessageBoxButton.YesNo);

    if (dialog == MessageBoxResult.No)
    {
      return;
    }

    _projectService.Close();
  }
}
