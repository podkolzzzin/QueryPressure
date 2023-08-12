using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;

public class ProjectNodeViewModel : BaseNodeViewModel, IDisposable
{
  private readonly ISubscription _subscription;
  private readonly INodeCreator _nodeCreator;
  private readonly ISubscription _childrenChangedSubscription;

  public ProjectNodeViewModel(ISubscriptionManager subscriptionManager, INodeCreator nodeCreator, ProjectModel projectModel) : base(projectModel, true)
  {
    _subscription = subscriptionManager
      .On(ModelAction.Edit, projectModel)
      .Subscribe(OnModelEdit);
    _nodeCreator = nodeCreator;

    _childrenChangedSubscription = subscriptionManager
      .On(ModelAction.ChildrenChanged, projectModel)
      .Subscribe(OnChildredChanged);

    BuildChildrenNodes();
  }

  private void OnChildredChanged(object? sender, IModel value)
  {
    Model = value;

    BuildChildrenNodes();
  }

  public void BuildChildrenNodes()
  {
    if (Nodes is null)
    {
      throw new ArgumentNullException(nameof(Nodes));
    }

    MergeNodes(Nodes, ProjectModel.Scenarios, (node, model) => node.Model.Id == model.Id, scenarioModel => _nodeCreator.Create(scenarioModel));
  }

  private void MergeNodes<TNode, TModel>(ObservableCollection<TNode> nodes, List<TModel> models, Func<TNode, TModel, bool> isEqualFunc, Func<TModel, TNode> createNodeFunc)
  {

    foreach (var item in models)
    {
      if (nodes.Any(x => isEqualFunc(x, item)))
      {
        continue;
      }

      var newNode = createNodeFunc(item);
      nodes.Add(newNode);
    }

    var toDelete = nodes.Where(node => models.All(model => !isEqualFunc(node, model))).ToList();

    foreach (var item in toDelete)
    {
      nodes.Remove(item);

      if (item is IDisposable disposableChild)
      {
        disposableChild.Dispose();
      }
    }
  }

  private void OnModelEdit(object? sender, IModel value)
  {
    Model = value;
    OnOtherPropertyChanged(nameof(Title));
  }

  public ProjectModel ProjectModel => (ProjectModel)Model;

  public string Title => ProjectModel.Name;

  public void Dispose()
  {
    _subscription.Dispose();
    _childrenChangedSubscription.Dispose();
  }
}
