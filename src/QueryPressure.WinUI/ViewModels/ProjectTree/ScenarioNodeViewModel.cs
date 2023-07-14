using System.Windows;
using System.Windows.Input;
using QueryPressure.WinUI.Commands.Scenario;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;

public class ScenarioNodeViewModel : BaseNodeViewModel, IDisposable
{
  private readonly OpenScenarioScriptCommand _openScenarioScriptCommand;
  private readonly INodeCreator _nodeCreator;
  private readonly ISubscription _subscription;
  private readonly ISubscription _scenarioExecutedSubscription;

  public ScenarioNodeViewModel(ISubscriptionManager subscriptionManager,
    OpenScenarioScriptCommand openScenarioScriptCommand, INodeCreator nodeCreator, ScenarioModel scenarioModel) : base(scenarioModel, true)
  {
    if (Nodes is null)
    {
      throw new ArgumentNullException(nameof(Nodes));
    }

    foreach (var executionNode in scenarioModel.Executions.Select(nodeCreator.Create))
    {
      Nodes.Add(executionNode);
    }

    _openScenarioScriptCommand = openScenarioScriptCommand;
    _nodeCreator = nodeCreator;
    _subscription = subscriptionManager
      .On(ModelAction.Edit, scenarioModel)
      .Subscribe(OnModelEdit);

    _scenarioExecutedSubscription = subscriptionManager
      .On(ModelAction.ChildrenChanged, scenarioModel)
      .Subscribe(OnScenarioExecuted);
  }

  private void OnScenarioExecuted(object? sender, IModel value)
  {
    var scenarioModel = value as ScenarioModel;
    var newExecutionModel = scenarioModel?.Executions?
      .SingleOrDefault(
        x => !Nodes?.OfType<ExecutionNodeViewModel>()
        .Select(node => node.Id).ToHashSet().Contains(x.Id) ?? false);

    if (newExecutionModel == null)
    {
      throw new InvalidOperationException();
    }

    var newNode = (ExecutionNodeViewModel)_nodeCreator.Create(newExecutionModel);
    Nodes?.Insert(0, newNode);
    OnOtherPropertyChanged(nameof(Nodes));

    IsExpanded = true;
    newNode.IsSelected = true;
    newNode.OpenExecution();
  }

  private void OnModelEdit(object? sender, IModel value)
  {
    Model = value;
    OnOtherPropertyChanged(nameof(Title));
  }

  public string Title => ScenarioModel.Name;

  public ScenarioModel ScenarioModel => (ScenarioModel)Model;

  public override void Click(MouseButtonEventArgs args, bool isDoubleClick = false)
  {
    var originalSource = (args.OriginalSource as FrameworkElement)?.DataContext;

    if (originalSource == this && isDoubleClick)
    {
      _openScenarioScriptCommand.Execute(ScenarioModel);
    }
  }

  public void Dispose()
  {
    _subscription.Dispose();
    _scenarioExecutedSubscription.Dispose();
  }
}
