using System.Windows;
using Microsoft.Win32;
using QueryPressure.WinUI.Common.Commands;

namespace QueryPressure.WinUI.Commands.Project;

public class DialogOpenProjectCommand : CommandBase
{
  private readonly OpenProjectCommand _openProjectCommand;

  public DialogOpenProjectCommand(OpenProjectCommand openProjectCommand)
  {
    _openProjectCommand = openProjectCommand;
  }

  public override bool CanExecute(object? parameter) => true;

  public override void Execute(object? parameter)
  {
    var dialog = new OpenFileDialog
    {
      AddExtension = false,
      CheckFileExists = true,
      CheckPathExists = true,
      DefaultExt = Constants.ProjectExtension,
      Filter = $"QueryPressure Project File (QPP) | *.{Constants.ProjectExtension}"
    };

    if (!(dialog.ShowDialog(Application.Current.MainWindow) ?? false))
    {
      return;
    }

    _openProjectCommand.Execute(dialog.FileName);
  }
}
