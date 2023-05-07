using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Win32;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Project;

namespace QueryPressure.WinUI.Commands.Project;

public class DialogSaveProjectCommand : CommandBase
{
  private readonly IProjectService _projectService;
  private readonly SaveProjectCommand _saveProjectCommand;

  public DialogSaveProjectCommand(IProjectService projectService, SaveProjectCommand saveProjectCommand)
  {
    _projectService = projectService;
    _saveProjectCommand = saveProjectCommand;
  }

  public override bool CanExecute(object? parameter) => _projectService.Project is not null;

  public override void Execute(object? parameter)
  {
    var basePath = Path.GetDirectoryName(_projectService.Project?.Path?.FullName) ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    var dialog = new SaveFileDialog
    {
      InitialDirectory = basePath,
      AddExtension = false,
      CheckFileExists = false,
      CheckPathExists = true,
      DefaultExt = Constants.ProjectExtension,
      Filter = $"QueryPressure Project File (QPP) | *.{Constants.ProjectExtension}"
    };

    if (!(dialog.ShowDialog(Application.Current.MainWindow) ?? false))
    {
      return;
    }

    _saveProjectCommand.Execute(dialog.FileName);
  }
}
