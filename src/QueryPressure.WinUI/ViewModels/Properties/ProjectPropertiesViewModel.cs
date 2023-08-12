using QueryPressure.WinUI.Commands.App;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.ViewModels.Properties;

public class ProjectPropertiesViewModel : BaseModelPropertiesViewModel<ProjectModel>, IDisposable
{
  private readonly ISubscription _subscription;
  private string? _name;
  private string? _path;

  public ProjectPropertiesViewModel(ISubscriptionManager subscriptionManager, EditModelCommand editModelCommand, ProjectModel projectModel)
    : base(editModelCommand, projectModel)
  {
    _subscription = subscriptionManager
      .On(ModelAction.Edit, projectModel)
      .Subscribe(OnModelEdit);

    OnModelEdit(null, projectModel);
  }

  private void OnModelEdit(object? sender, IModel value)
  {
    if (sender == this)
    {
      return;
    }

    var model = (ProjectModel)value;
    Name = model.Name;
    Path = model.Path?.FullName ?? "<not set>";
  }

  public string? Name
  {
    get => _name;
    set => SetModelField(ref _name, value);
  }

  public string? Path
  {
    get => _path;
    private set => SetField(ref _path, value);
  }

  public void Dispose()
  {
    _subscription.Dispose();
  }
}
