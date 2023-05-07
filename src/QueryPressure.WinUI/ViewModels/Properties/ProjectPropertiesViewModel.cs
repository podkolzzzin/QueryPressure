using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.ViewModels.Properties;

public class ProjectPropertiesViewModel : ViewModelBase, IDisposable
{
  private readonly ISubscription _subscription;
  private string? _name;
  private string? _path;

  public ProjectPropertiesViewModel(ISubscriptionManager subscriptionManager, ProjectModel projectModel)
  {
    _subscription = subscriptionManager
      .On(ModelAction.Edit, projectModel)
      .Subscribe(OnModelEdit);

    OnModelEdit(projectModel);
  }

  private void OnModelEdit(IModel value)
  {
    var model = (ProjectModel) value;
    Name = model.Name;
    Path = model.Path?.FullName ?? "<not set>";
  }

  public string? Name
  {
    get => _name;
    set => SetField(ref _name, value);
  }

  public string? Path
  {
    get => _path;
    set => SetField(ref _path, value);
  }

  public void Dispose()
  {
    _subscription.Dispose();
  }
}
