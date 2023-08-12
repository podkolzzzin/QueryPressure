using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Commands.App;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Subscriptions;
using QueryPressure.WinUI.ViewModels;

namespace QueryPressure.WinUI.Commands.Scenario;

public class OpenScenarioScriptCommand : OpenDocumentCommand<ScenarioModel, ScriptViewModel>
{
  private readonly ISubscriptionManager _subscriptionManager;
  private readonly ILanguageService _languageService;
  private readonly EditModelCommand _editModelCommand;
  private readonly CloseScenarioScriptCommand _closeScenarioScriptCommand;

  public OpenScenarioScriptCommand(ILogger<OpenScenarioScriptCommand> logger,
    ISubscriptionManager subscriptionManager,
    ILanguageService languageService,
    DockToolsViewModel dockToolsViewModel,
    EditModelCommand editModelCommand,
    CloseScenarioScriptCommand closeScenarioScriptCommand) : base(logger, dockToolsViewModel)
  {
    _subscriptionManager = subscriptionManager;
    _languageService = languageService;
    _closeScenarioScriptCommand = closeScenarioScriptCommand;
    _editModelCommand = editModelCommand;
  }

  protected override ScriptViewModel CreateViewModel(ScenarioModel model)
   => new ScriptViewModel(_subscriptionManager, _languageService, _editModelCommand, _closeScenarioScriptCommand, model);
}
