using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.ViewModels;

namespace QueryPressure.WinUI.Commands.Scenario;

public class CloseScenarioScriptCommand : CommandBase
{
  private readonly DockToolsViewModel _dockToolsViewModel;

  public CloseScenarioScriptCommand(DockToolsViewModel dockToolsViewModel)
  {
    _dockToolsViewModel = dockToolsViewModel;
  }

  public override bool CanExecute(object? parameter) => true;

  protected override void ExecuteInternal(object? parameter)
  {
    if (parameter is not ScenarioModel scenario)
    {
      throw new ArgumentNullException(nameof(scenario));
    }

    var fileViewModel = _dockToolsViewModel.Files.OfType<ScriptViewModel>().FirstOrDefault(fm => fm.IsEqualTo(scenario));

    if (fileViewModel == null)
    {
      throw new ArgumentNullException(nameof(scenario));
    }

    _dockToolsViewModel.Files.Remove(fileViewModel);
    fileViewModel.Dispose();
  }
}