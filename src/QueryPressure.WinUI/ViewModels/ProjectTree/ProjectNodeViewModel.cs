using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;
public class ProjectNodeViewModel : BaseNodeViewModel, IDisposable
{
  private readonly ISubscription _subscription;

  public ProjectNodeViewModel(ISubscriptionManager subscriptionManager, ProjectModel projectModel) : base(projectModel, true)
  {
    if (Nodes is null)
    {
      throw new ArgumentNullException(nameof(Nodes));
    }

    foreach (var profileNode in projectModel.Profiles.Select(profile => new ProfileNodeViewModel(subscriptionManager, profile)))
    {
      Nodes.Add(profileNode);
    }
    
    _subscription = subscriptionManager
      .On(ModelAction.Edit, projectModel)
      .Subscribe(OnModelEdit);
  }

  private void OnModelEdit(IModel value)
  {
    Model = value;
    OnOtherPropertyChanged(nameof(Title));
  }

  private ProjectModel ProjectModel => (ProjectModel)Model;

  public string Title => ProjectModel.Name;

  public void Dispose()
  {
    _subscription.Dispose();
  }
}
