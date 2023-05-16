using System.Windows.Input;
using QueryPressure.WinUI.Commands.Scenario;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Subscriptions;
using QueryPressure.WinUI.ViewModels.DockElements;

namespace QueryPressure.WinUI.ViewModels;

public class ScriptViewModel : PaneViewModel, IDisposable
{
  private readonly IDisposable _subscription;
  private readonly ILanguageService _languageService;

  private ScenarioModel _model;
  private string? _script;


  public ScriptViewModel(ISubscriptionManager subscriptionManager, ILanguageService languageService, CloseScenarioScriptCommand closeScenarioScriptCommand, ScenarioModel scenarioModel)
  {
    _languageService = languageService;

    _subscription = subscriptionManager
      .On(ModelAction.Edit, scenarioModel)
      .Subscribe(OnScenarioChanged);

    _model = scenarioModel;

    ContentId = _model.Id.ToString();
    OnScenarioChanged(_model);
    CloseCommand = new DelegateCommand(() => closeScenarioScriptCommand.Execute(_model));
  }

  private void OnScenarioChanged(IModel value)
  {
    _model = (ScenarioModel)value;

    if (!ContentId.Equals(_model.Id.ToString()))
    {
      throw new InvalidOperationException("Content ID has changed");
    }

    Script = _model.Script ?? string.Empty;

    var strings = _languageService.GetStrings();

    Title = $"{_model.Name} - {strings["labels.scenario.script"]}";
  }

  public ICommand CloseCommand { get; }

  public string? Script
  {
    get => _script;
    set => SetField(ref _script, value);
  }

  public bool IsEqualTo(ScenarioModel scenarioModel)
  {
    return scenarioModel.Id.ToString() == ContentId;
  }

  public void Dispose()
  {
    _subscription.Dispose();
  }
}
