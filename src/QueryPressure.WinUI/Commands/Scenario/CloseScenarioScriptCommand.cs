using QueryPressure.WinUI.Commands.App;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.ViewModels;

namespace QueryPressure.WinUI.Commands.Scenario;

public class CloseScenarioScriptCommand : CloseDocumentCommand<ScenarioModel, ScriptViewModel>
{
  public CloseScenarioScriptCommand(DockToolsViewModel dockToolsViewModel) : base(dockToolsViewModel)
  {
  }
}
