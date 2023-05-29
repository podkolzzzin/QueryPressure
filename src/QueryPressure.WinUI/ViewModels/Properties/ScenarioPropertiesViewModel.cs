using QueryPressure.Core.Interfaces;
using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.ViewModels.Properties;

public class ScenarioPropertiesViewModel : ViewModelBase, IDisposable
{
  private readonly ISubscription _subscription;
  private readonly ISubscription _scenarioExecutedSubscription;
  private string? _name;
  private bool? _canEdit;
  private string? _connectionString;
  private string? _provider;


  public ScenarioPropertiesViewModel(ISubscriptionManager subscriptionManager, ScenarioModel scenarioModel, IProviderInfo[] providers)
  {
    Providers = providers.Select(x => x.Name).ToArray();
    Provider = Providers.FirstOrDefault();

    _subscription = subscriptionManager
      .On(ModelAction.Edit, scenarioModel)
      .Subscribe(OnModelEdit);

    _scenarioExecutedSubscription = subscriptionManager
      .On(ModelAction.ChildrenChanged, scenarioModel)
      .Subscribe(OnScenarioExecuted);

    OnModelEdit(null, scenarioModel);
    OnScenarioExecuted(null, scenarioModel);
  }

  public string[] Providers { get; set; }

  private void OnScenarioExecuted(object? sender, IModel value)
  {
    var model = (ScenarioModel)value;
    CanEdit = !model.IsReadOnly;
  }

  private void OnModelEdit(object? sender, IModel value)
  {
    var model = (ScenarioModel)value;
    Name = model.Name;
    ConnectionString = model.ConnectionString;
    Provider = model.Provider;
  }

  public bool? CanEdit
  {
    get => _canEdit;
    set => SetField(ref _canEdit, value);
  }

  public string? Name
  {
    get => _name;
    set => SetField(ref _name, value);
  }

  public string? Provider
  {
    get => _provider;
    set => SetField(ref _provider, value);
  }

  public string? ConnectionString
  {
    get => _connectionString;
    set => SetField(ref _connectionString, value);
  }

  public void Dispose()
  {
    _subscription.Dispose();
    _scenarioExecutedSubscription.Dispose();
  }
}
