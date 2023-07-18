using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Commands.App;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Subscriptions;
using QueryPressure.WinUI.ViewModels;
using QueryPressure.WinUI.ViewModels.Execution;
using QueryPressure.WinUI.ViewModels.Helpers.Status;

namespace QueryPressure.WinUI.Commands.Execution;

public class OpenExecutionResultsCommand : OpenDocumentCommand<ExecutionModel, ExecutionViewModel>
{
  private readonly ISubscriptionManager _subscriptionManager;
  private readonly ILanguageService _languageService;
  private readonly IExecutionStatusProvider _executionStatusProvider;
  private readonly CloseExecutionResultsCommand _closeExecutionResultsCommand;

  public OpenExecutionResultsCommand(ILogger<OpenExecutionResultsCommand> logger,
    ISubscriptionManager subscriptionManager,
    ILanguageService languageService,
    IExecutionStatusProvider executionStatusProvider,
    DockToolsViewModel dockToolsViewModel,
    CloseExecutionResultsCommand closeExecutionResultsCommand
) : base(logger, dockToolsViewModel)
  {
    _subscriptionManager = subscriptionManager;
    _languageService = languageService;
    _closeExecutionResultsCommand = closeExecutionResultsCommand;
    _executionStatusProvider = executionStatusProvider;
  }

  protected override ExecutionViewModel CreateViewModel(ExecutionModel model)
   => new ExecutionViewModel(_subscriptionManager, _languageService, _executionStatusProvider, _closeExecutionResultsCommand, model);
}
