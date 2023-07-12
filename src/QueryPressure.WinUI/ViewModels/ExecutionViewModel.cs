using System.Windows.Input;
using QueryPressure.WinUI.Commands.Execution;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Subscriptions;
using QueryPressure.WinUI.ViewModels.DockElements;

namespace QueryPressure.WinUI.ViewModels;

public class ExecutionViewModel : PaneViewModel, IDisposable
{
  private readonly IDisposable _subscription;
  private readonly ILanguageService _languageService;
  private ExecutionModel? _model;

  public ExecutionViewModel(ISubscriptionManager subscriptionManager, ILanguageService languageService,
    CloseExecutionResultsCommand closeExecutionResultsCommand, ExecutionModel executionModel)
  {
    _languageService = languageService;

    ContentId = executionModel.Id.ToString();
    OnExecutionChanged(null, executionModel);

    _subscription = subscriptionManager
      .On(ModelAction.Edit, executionModel)
      .Subscribe(OnExecutionChanged);

    CloseCommand = new DelegateCommand(() => closeExecutionResultsCommand.Execute(_model));
  }

  private void OnExecutionChanged(object? sender, IModel value)
  {
    if (sender == this)
    {
      return;
    }

    _model = (ExecutionModel)value;

    if (!ContentId.Equals(_model.Id.ToString()))
    {
      throw new InvalidOperationException("Content ID has changed");
    }

    var strings = _languageService.GetStrings();
    Title = $"{_model.Title} - {strings["labels.execution.results"]}";
  }

  public string FilePath => Title;

  public void Dispose()
  {
    _subscription.Dispose();
  }
}
