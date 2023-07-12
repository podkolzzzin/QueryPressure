using System.Windows.Input;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using QueryPressure.WinUI.Commands.App;
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
  private readonly EditModelCommand _editModelCommand;
  private ScenarioModel? _model;
  private TextDocument _document;
  private bool _isDirty;
  private IHighlightingDefinition? _highlightingDefinition;


  public ScriptViewModel(ISubscriptionManager subscriptionManager, ILanguageService languageService,
    EditModelCommand editModelCommand,
    CloseScenarioScriptCommand closeScenarioScriptCommand, ScenarioModel scenarioModel)
  {
    _languageService = languageService;
    _editModelCommand = editModelCommand;
    _document = new TextDocument();
    _document.Changed += _document_Changed;

    ContentId = scenarioModel.Id.ToString();
    OnScenarioChanged(null, scenarioModel);

    _subscription = subscriptionManager
      .On(ModelAction.Edit, scenarioModel)
      .Subscribe(OnScenarioChanged);

    CloseCommand = new DelegateCommand(() => closeScenarioScriptCommand.Execute(_model));
  }

  private void _document_Changed(object? sender, DocumentChangeEventArgs e)
  {
    if (_model == null)
    {
      return;
    }

    _model.Script = _document.Text;
    _editModelCommand.DeBounce(new EditModelCommandParameter(this, _model));
  }

  private void OnScenarioChanged(object? sender, IModel value)
  {
    if (sender == this)
    {
      return;
    }

    _model = (ScenarioModel)value;

    if (!ContentId.Equals(_model.Id.ToString()))
    {
      throw new InvalidOperationException("Content ID has changed");
    }

    HighlightingDefinition = GetHighlightingDefinition(_model);

    var modelText = _model.Script ?? string.Empty;

    if (!modelText.Equals(Document.Text))
    {
      Document.Text = modelText;
    }

    var strings = _languageService.GetStrings();
    Title = $"{_model.Name} - {strings["labels.scenario.script"]}";
  }

  private static IHighlightingDefinition GetHighlightingDefinition(ScenarioModel model)
    => (model.Profile) switch
    {
      _ => HighlightingManager.Instance.GetDefinition("TSQL")
    };

  public string FilePath => Title;

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

  public IHighlightingDefinition? HighlightingDefinition
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
