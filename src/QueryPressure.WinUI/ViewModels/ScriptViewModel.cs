using System.Windows.Input;
using QueryPressure.WinUI.Commands.Scenario;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.ViewModels.DockElements;

namespace QueryPressure.WinUI.ViewModels;

public class ScriptViewModel : PaneViewModel
{
  private string _script;

  public ScriptViewModel(CloseScenarioScriptCommand closeScenarioScriptCommand, ScenarioModel scenarioModel)
  {
    _script = scenarioModel.Script ?? string.Empty;
    Title = GetTitle(scenarioModel);
    ContentId = scenarioModel.Id.ToString();
    CloseCommand = new DelegateCommand(() => closeScenarioScriptCommand.Execute(scenarioModel));
  }

  public ICommand CloseCommand { get; }

  private string GetTitle(ScenarioModel scenarioModel)
  {
    return $"{scenarioModel.Name} - Script";
  }

  public string Script
  {
    get => _script;
    set => SetField(ref _script, value);
  }

  public bool IsEqualTo(ScenarioModel scenarioModel)
  {
    return scenarioModel.Id.ToString() == ContentId;
  }
}
