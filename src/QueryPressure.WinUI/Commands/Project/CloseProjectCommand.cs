using System.Windows;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Project;

namespace QueryPressure.WinUI.Commands.Project;

public class CloseProjectCommand : CommandBase
{
  private readonly IProjectService _projectService;
  private readonly ILanguageService _languageService;

  public CloseProjectCommand(IProjectService projectService, ILanguageService languageService)
  {
    _projectService = projectService;
    _languageService = languageService;
  }

  public override bool CanExecute(object? parameter) => _projectService.Project is not null;

  protected override void ExecuteInternal(object? parameter)
  {
    var strings = _languageService.GetStrings();
    var dialog = MessageBox.Show(
      strings["labels.dialogs.confirm-close-project.message"],
      strings["labels.dialogs.confirm-close-project.title"],
      MessageBoxButton.YesNo);

    if (dialog == MessageBoxResult.No)
    {
      return;
    }

    _projectService.Close();
  }
}
