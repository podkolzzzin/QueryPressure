using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.WinUI.Commands.App;
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
  private readonly EditModelCommand _editModelCommand;
  private readonly IProviderInfo[] _providers;
  private readonly IProfileCreator[] _profileCreators;
  private readonly ILimitCreator[] _limitCreators;
  private readonly ISubscription _selectionSubscription;
  private ViewModelBase? _content;

  public PropertiesViewModel(ISubscriptionManager subscriptionManager, IObservableItem<Selection> selectionObservable,
    EditModelCommand editModelCommand, IProviderInfo[] providers, IProfileCreator[] profileCreators, ILimitCreator[] limitCreators) : base("properties")
  {
    _subscriptionManager = subscriptionManager;
    _editModelCommand = editModelCommand;
    _providers = providers;
    _profileCreators = profileCreators;
    _limitCreators = limitCreators;
    _selectionSubscription = selectionObservable.Subscribe(UpdateContentViewModel);
    UpdateContentViewModel(null, selectionObservable.CurrentValue);
  }

  private void UpdateContentViewModel(object? sender, Selection selection)
  {
    if (Content is IDisposable disposable)
    {
      disposable.Dispose();
    }

    Content = selection.Model switch
    {
      ProjectModel projectModel => new ProjectPropertiesViewModel(_subscriptionManager, _editModelCommand, projectModel),
      ScenarioModel profileModel => new ScenarioPropertiesViewModel(_subscriptionManager, _editModelCommand, profileModel, _providers, _profileCreators, _limitCreators),
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
