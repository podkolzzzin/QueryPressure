using QueryPressure.WinUI.Commands.Scenario;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Selection;
using QueryPressure.WinUI.Services.Subscriptions;
using QueryPressure.WinUI.ViewModels.DockElements;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;

public class ProjectTreeViewModel : ToolViewModel, IDisposable
{
  private readonly ISubscriptionManager _subscriptionManager;
  private readonly ISelectionService _selectionService;
  private readonly INodeCreator _nodeCreator;
  private readonly ISubscription _subscription;

  public ProjectTreeViewModel(IObservableItem<ProjectModel?> projectObserver, ISubscriptionManager subscriptionManager,
    ISelectionService selectionService, INodeCreator nodeCreator) : base("project-tree")
  {
    _subscriptionManager = subscriptionManager;
    _selectionService = selectionService;
    _nodeCreator = nodeCreator;
    Nodes = new List<BaseNodeViewModel>();
    _subscription = projectObserver.Subscribe(BuildTree);
  }

  public void BuildTree(ProjectModel? project)
  {
    if (Nodes.Any())
    {
      CleanTree(Nodes);
    }

    Nodes = new List<BaseNodeViewModel>();

    if (project is null)
    {
      OnOtherPropertyChanged(nameof(Nodes));
      return;
    }

    Nodes.Add(_nodeCreator.Create(project));
    OnOtherPropertyChanged(nameof(Nodes));
  }

  private static void CleanTree(List<BaseNodeViewModel> nodes)
  {
    foreach (var node in nodes)
    {
      if (node.Nodes != null)
      {
        CleanTree(node.Nodes);
      }

      if (node is IDisposable disposable)
      {
        disposable.Dispose();
      }
    }
  }

  public List<BaseNodeViewModel> Nodes { get; private set; }

  public void Dispose()
  {
    _subscription.Dispose();
  }

  internal void SelectedNode(BaseNodeViewModel? nodeViewModel)
  {
    _selectionService.Set(nodeViewModel?.Model);
  }
}
