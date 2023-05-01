using System.Collections.ObjectModel;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;
using QueryPressure.WinUI.ViewModels.DockElements;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;

public class ProjectTreeViewModel : ToolViewModel, IDisposable
{
  private readonly ISubscriptionManager _subscriptionManager;
  private readonly ISubscription _subscription;

  public ProjectTreeViewModel(IObservableItem<ProjectModel?> projectObserver, ISubscriptionManager subscriptionManager) : base("project-tree")
  {
    _subscriptionManager = subscriptionManager;
    Nodes = new ObservableCollection<BaseNodeViewModel>();
    _subscription = projectObserver.Subscribe(BuildTree);
  }
  
  public void BuildTree(ProjectModel? project)
  {
    Nodes.Clear();

    if (project is null)
    {
      return;
    }
    
    Nodes.Add(new ProjectNodeViewModel(_subscriptionManager, project));
  }

  public ObservableCollection<BaseNodeViewModel> Nodes { get; private set; }

  public void Dispose()
  {
    _subscription.Dispose();
  }
}
