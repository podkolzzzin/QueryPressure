using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.ViewModels;

namespace QueryPressure.WinUI.Commands.Scenario;

public class CloseScenarioScriptCommand : CommandBase
{
  private readonly DockToolsViewModel _dockToolsViewModel;

  public CloseScenarioScriptCommand(ILogger<CloseScenarioScriptCommand> logger, DockToolsViewModel dockToolsViewModel) 
  {
    _dockToolsViewModel = dockToolsViewModel;
  }


  public override bool CanExecute(object? parameter)
  {
    return true;
  }

  protected override void ExecuteInternal(object? parameter)
  {
    if (parameter is not ScenarioModel scenario)
    {
      throw new ArgumentNullException(nameof(scenario));
    }

    var fileViewModel = _dockToolsViewModel.Files.FirstOrDefault(fm => fm.IsEqualTo(scenario));

    if (fileViewModel == null)
    {
      throw new ArgumentNullException(nameof(scenario));
    }

    _dockToolsViewModel.Files.Remove(fileViewModel);
    fileViewModel.Dispose();
  }
}
