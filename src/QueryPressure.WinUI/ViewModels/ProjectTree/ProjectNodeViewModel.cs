using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;
public class ProjectNodeViewModel : BaseNodeViewModel, IDisposable
{
  private readonly ISubscription _subscription;

  public ProjectNodeViewModel(ISubscriptionManager subscriptionManager, ProjectModel projectModel) : base(null, projectModel)
  {
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
