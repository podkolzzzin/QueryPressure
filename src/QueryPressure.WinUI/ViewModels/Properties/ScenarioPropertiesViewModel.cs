using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.WinUI.Commands.App;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.ViewModels.Properties;

public class ScenarioPropertiesViewModel : BaseModelPropertiesViewModel<ScenarioModel>, IDisposable
{
  private readonly ISubscription _subscription;
  private readonly ISubscription _scenarioExecutedSubscription;
  private string? _name;
  private bool _canEdit;
  private string? _connectionString;
  private string? _provider;

  public ScenarioPropertiesViewModel(ISubscriptionManager subscriptionManager, EditModelCommand editModelCommand,
    ScenarioModel scenarioModel, IProviderInfo[] providers, IProfileCreator[] profileCreators, ILimitCreator[] limitCreators)
    : base(editModelCommand, scenarioModel)
  {
    Providers = providers.Select(x => x.Name).ToArray();

    Profile = new ArgumentsPropertiesViewModel(profileCreators.Select(x => x.Type).ToArray(),
      profileCreators.ToDictionary(x => x.Type, x => x.Arguments), "profiles", UpdateProfile);

    Limit = new ArgumentsPropertiesViewModel(limitCreators.Select(x => x.Type).ToArray(),
      limitCreators.ToDictionary(x => x.Type, x => x.Arguments), "limits", UpdateLimit);

    _subscription = subscriptionManager
      .On(ModelAction.Edit, scenarioModel)
      .Subscribe(OnModelEdit);

    _scenarioExecutedSubscription = subscriptionManager
      .On(ModelAction.ChildrenChanged, scenarioModel)
      .Subscribe(OnScenarioExecuted);

    OnModelEdit(null, scenarioModel);
    OnScenarioExecuted(null, scenarioModel);
  }

  private void UpdateLimit(SetModelType type)
  {
    NotifyEditModel(type, (model, value) =>
    {
      model.Limit = value;
      return model;
    }, Limit.GetArguments());
  }

  private void UpdateProfile(SetModelType type)
  {
    NotifyEditModel(type, (model, value) =>
    {
      model.Profile = value;
      return model;
    }, Profile.GetArguments());
  }

  public string[] Providers { get; }

  public bool CanEdit
  {
    get => _canEdit;
    set => SetField(ref _canEdit, value);
  }

  public string? Name
  {
    get => _name;
    set => SetModelField(ref _name, value);
  }

  public string? Provider
  {
    get => _provider;
    set => SetModelField(ref _provider, value);
  }

  public string? ConnectionString
  {
    get => _connectionString;
    set => SetModelField(ref _connectionString, value);
  }

  public ArgumentsPropertiesViewModel Profile { get; }
  public ArgumentsPropertiesViewModel Limit { get; }

  private void OnScenarioExecuted(object? sender, IModel value)
  {
    var model = (ScenarioModel)value;
    CanEdit = !model.IsReadOnly;
  }

  private void OnModelEdit(object? sender, IModel value)
  {
    if (sender == this)
    {
      return;
    }

    var model = (ScenarioModel)value;
    Name = model.Name;
    ConnectionString = model.ConnectionString;
    Provider = model.Provider;
    Profile.SetValue(model.Profile);
    Limit.SetValue(model.Limit);
  }

  public void Dispose()
  {
    _subscription.Dispose();
    _scenarioExecutedSubscription.Dispose();
  }
}
