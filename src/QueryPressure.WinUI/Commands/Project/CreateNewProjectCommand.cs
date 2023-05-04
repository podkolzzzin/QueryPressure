using System.Windows;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Project;
using QueryPressure.WinUI.ViewModels.Helpers;

namespace QueryPressure.WinUI.Commands.Project;

public class CreateNewProjectCommand : CommandBase
{
  private readonly IProjectService _projectService;
  private readonly LocaleViewModel _locale;
  private readonly CloseProjectCommand _closeProjectCommand;

  public CreateNewProjectCommand(IProjectService projectService, LocaleViewModel locale, CloseProjectCommand closeProjectCommand)
  {
    _projectService = projectService;
    _locale = locale;
    _closeProjectCommand = closeProjectCommand;
  }

  public override bool CanExecute(object? parameter) => true;

  public override void Execute(object? parameter)
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
