using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.ViewModels;
using SharpCompress.Common;

namespace QueryPressure.WinUI.Commands.Scenario;

public class OpenScenarioScriptCommand :  CommandBase<ScenarioModel>
{
  private readonly DockToolsViewModel _dockToolsViewModel;
  private readonly CloseScenarioScriptCommand _closeScenarioScriptCommand;

  public OpenScenarioScriptCommand(ILogger<OpenScenarioScriptCommand> logger,
    DockToolsViewModel dockToolsViewModel,
    CloseScenarioScriptCommand closeScenarioScriptCommand) : base(logger)
  {
    _dockToolsViewModel = dockToolsViewModel;
    _closeScenarioScriptCommand = closeScenarioScriptCommand;
  }

  protected override void ExecuteInternal(ScenarioModel scenario)
  {
    var scenarioViewModel = Open(scenario);
    _dockToolsViewModel.ActiveDocument = scenarioViewModel;
  }

  private ScriptViewModel Open(ScenarioModel scenario)
  {
    var fileViewModel = _dockToolsViewModel.Files.FirstOrDefault(fm => fm.IsEqualTo(scenario));

    if (fileViewModel != null)
    {
      return fileViewModel;
    }


    fileViewModel = new ScriptViewModel(_closeScenarioScriptCommand, scenario);
    _dockToolsViewModel.Files.Add(fileViewModel);
    return fileViewModel;
  }
}
