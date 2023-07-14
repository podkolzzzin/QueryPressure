using QueryPressure.WinUI.Commands.Scenario;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;
using System.Windows.Input;
using System.Windows;
using QueryPressure.WinUI.Commands.Execution;
using QueryPressure.WinUI.ViewModels.Helpers.Status;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;

public class ExecutionNodeViewModel : BaseNodeViewModel, IDisposable
{
  private ISubscription _subscription;
  private readonly OpenExecutionResultsCommand _openExecutionResultsCommand;
  private readonly IExecutionStatusProvider _executionStatusProvider;

  public ExecutionNodeViewModel(ISubscriptionManager subscriptionManager,
    OpenExecutionResultsCommand openExecutionResultsCommand, IExecutionStatusProvider executionStatusProvider, IModel model) : base(model, false)
  {
    _subscription = subscriptionManager
      .On(ModelAction.Edit, model)
      .Subscribe(OnModelChanged);
    _openExecutionResultsCommand = openExecutionResultsCommand;
    _executionStatusProvider = executionStatusProvider;

    Status = _executionStatusProvider.GetStatus(ExecutionStatus.None);
  }

  private void OnModelChanged(object? sender, IModel value)
  {
    var executionModel = value as ExecutionModel;
    Status = _executionStatusProvider.GetStatus(executionModel?.Status ?? ExecutionStatus.None);
    Title = executionModel?.Title;

    OnPropertyChanged(nameof(Status));
    OnPropertyChanged(nameof(Title));
  }

  public Guid Id => Model.Id;


  public string? Title { get; private set; }
  public ExecutionStatusViewModel Status { get; private set; }

  public ExecutionModel ExecutionModel => (ExecutionModel)Model;

  public override void Click(MouseButtonEventArgs args, bool isDoubleClick = false)
  {
    var originalSource = (args.OriginalSource as FrameworkElement)?.DataContext;

    if (originalSource == this && isDoubleClick)
    {
      OpenExecution();
    }
  }

  public void OpenExecution()
  {
    _openExecutionResultsCommand.Execute(ExecutionModel);
  }

  public void Dispose()
  {
    _subscription.Dispose();
  }
}
