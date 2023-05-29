using System.Windows.Input;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
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

  private ScenarioModel? _model;
  private TextDocument? _document;
  private bool _isDirty;
  private IHighlightingDefinition? _highlightingDefinition;


  public ScriptViewModel(ISubscriptionManager subscriptionManager, ILanguageService languageService, CloseScenarioScriptCommand closeScenarioScriptCommand, ScenarioModel scenarioModel)
  {
    _languageService = languageService;

    _subscription = subscriptionManager
      .On(ModelAction.Edit, scenarioModel)
      .Subscribe(OnScenarioChanged);

    ContentId = scenarioModel.Id.ToString();
    OnScenarioChanged(scenarioModel);
    CloseCommand = new DelegateCommand(() => closeScenarioScriptCommand.Execute(_model));
  }

  private void OnScenarioChanged(IModel value)
  {
    _model = (ScenarioModel)value;

    if (!ContentId.Equals(_model.Id.ToString()))
    {
      throw new InvalidOperationException("Content ID has changed");
    }

    HighlightingDefinition = GetHighlightingDefinition(_model);
    Document = new TextDocument(_model.Script ?? string.Empty);

    var strings = _languageService.GetStrings();

    Title = $"{_model.Name} - {strings["labels.scenario.script"]}";
  }

  private static IHighlightingDefinition GetHighlightingDefinition(ScenarioModel model)
    => (model.Profile) switch
    {
      _ => HighlightingManager.Instance.GetDefinition("TSQL")
    };

  public ICommand CloseCommand { get; }

  public TextDocument Document
  {
    get => _document;
    set => SetField(ref _document, value);
  }

  public bool IsDirty
  {
    get => _isDirty;
    set => SetField(ref _isDirty, value);
  }

  public IHighlightingDefinition HighlightingDefinition
  {
    get => _highlightingDefinition;
    set => SetField(ref _highlightingDefinition, value);
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
