using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Subscriptions;
using QueryPressure.WinUI.ViewModels;
using QueryPressure.WinUI.ViewModels.Execution;
using QueryPressure.WinUI.ViewModels.Helpers.Status;

namespace QueryPressure.WinUI.Commands.Execution;

public class OpenExecutionResultsCommand : CommandBase<ExecutionModel>
{
  private readonly ISubscriptionManager _subscriptionManager;
  private readonly ILanguageService _languageService;
  private readonly DockToolsViewModel _dockToolsViewModel;
  private readonly CloseExecutionResultsCommand _closeExecutionResultsCommand;
  private readonly IExecutionStatusProvider _executionStatusProvider;

  public OpenExecutionResultsCommand(ILogger<OpenExecutionResultsCommand> logger,
    ISubscriptionManager subscriptionManager,
    ILanguageService languageService,
    DockToolsViewModel dockToolsViewModel,
    CloseExecutionResultsCommand closeExecutionResultsCommand,
    IExecutionStatusProvider executionStatusProvider) : base(logger)
  {
    _subscriptionManager = subscriptionManager;
    _languageService = languageService;
    _dockToolsViewModel = dockToolsViewModel;
    _closeExecutionResultsCommand = closeExecutionResultsCommand;
    _executionStatusProvider = executionStatusProvider;
  }

  protected override void ExecuteInternal(ExecutionModel execution)
  {
    var executionViewModel = Open(execution);
    _dockToolsViewModel.ActiveDocument = executionViewModel;
  }

  private ExecutionViewModel Open(ExecutionModel execution)
  {
    var fileViewModel = _dockToolsViewModel.Files.OfType<ExecutionViewModel>().FirstOrDefault(fm => fm.IsEqualTo(execution));

    if (fileViewModel != null)
    {
      return fileViewModel;
    }

    fileViewModel = new ExecutionViewModel(_subscriptionManager, _languageService, _closeExecutionResultsCommand, execution, _executionStatusProvider);
    _dockToolsViewModel.Files.Add(fileViewModel);
    return fileViewModel;
  }
}
