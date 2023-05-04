using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Selection;
using QueryPressure.WinUI.Services.Subscriptions;
using QueryPressure.WinUI.ViewModels.DockElements;

namespace QueryPressure.WinUI.ViewModels.Properties;
public class PropertiesViewModel : ToolViewModel, IDisposable
{
  private readonly ISubscriptionManager _subscriptionManager;
  private readonly ISubscription _selectionSubscription;
  private ViewModelBase? _content;

  public PropertiesViewModel(ISubscriptionManager subscriptionManager, IObservableItem<Selection> selectionObservable) : base("properties")
  {
    _subscriptionManager = subscriptionManager;
    _selectionSubscription = selectionObservable.Subscribe(UpdateContentViewModel);
    UpdateContentViewModel(selectionObservable.CurrentValue);
  }

  private void UpdateContentViewModel(Selection selection)
  {
    if (Content is IDisposable disposable)
    {
      disposable.Dispose();
    }

    Content = selection.Model switch
    {
      ProjectModel projectModel => new ProjectPropertiesViewModel(_subscriptionManager, projectModel),
      ProfileModel profileModel => new ProfilePropertiesViewModel(_subscriptionManager, profileModel),
      _ => null
    };
  }

  public ViewModelBase? Content
  {
    get => _content;
    set => SetField(ref _content, value);
  }

  public void Dispose()
  {
    _selectionSubscription.Dispose();
  }
}
