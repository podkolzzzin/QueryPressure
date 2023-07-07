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
  private readonly Dictionary<string, ArgumentDescriptor[]> _profileArgumentsMapper;
  private readonly Dictionary<string, ArgumentDescriptor[]> _limitArgumentsMapper;

  private readonly ISubscription _subscription;
  private readonly ISubscription _scenarioExecutedSubscription;
  private string? _name;
  private bool _canEdit;
  private string? _connectionString;
  private string? _provider;
  private string? _profile;
  private string? _limit;
  private ArgumentViewModel[]? _profileArguments;
  private ArgumentViewModel[]? _limitArguments;

  public ScenarioPropertiesViewModel(ISubscriptionManager subscriptionManager, EditModelCommand editModelCommand,
    ScenarioModel scenarioModel, IProviderInfo[] providers, IProfileCreator[] profileCreators, ILimitCreator[] limitCreators)
    : base(editModelCommand, scenarioModel)
  {
    Providers = providers.Select(x => x.Name).ToArray();

    _profileArgumentsMapper = profileCreators.ToDictionary(x => x.Type, x => x.Arguments);
    Profiles = profileCreators.Select(x => x.Type).ToArray();

    _limitArgumentsMapper = limitCreators.ToDictionary(x => x.Type, x => x.Arguments);
    Limits = limitCreators.Select(x => x.Type).ToArray();

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
  public string[] Profiles { get; set; }
  public string[] Limits { get; set; }

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
    Profile = model.Profile.Type!;
    ProfileArguments = BuildArguments($"profiles.{Profile}", _profileArgumentsMapper[Profile], model.Profile.Arguments).ToArray();
    Limit = model.Limit.Type!;
    LimitArguments = BuildArguments($"limits.{Limit}", _limitArgumentsMapper[Limit], model.Limit.Arguments).ToArray();
  }

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

  public string? Profile
  {
    get => _profile;
    set => SetModelField(ref _profile, value, UpdateProfile);
  }

  public ArgumentViewModel[]? ProfileArguments
  {
    get => _profileArguments;
    set => SetField(ref _profileArguments, value);
  }

  public string? Limit
  {
    get => _limit;
    set => SetModelField(ref _limit, value, UpdateLimit);
  }

  public ArgumentViewModel[]? LimitArguments
  {
    get => _limitArguments;
    set => SetField(ref _limitArguments, value);
  }

  private ScenarioModel UpdateProfile(ScenarioModel model, string? value)
  {
    if (_profileArguments != null)
    {
      model.Profile = GetArguments(_profile, _profileArguments);
    }

    return model;
  }

  private ScenarioModel UpdateLimit(ScenarioModel model, string? value)
  {
    if (_limitArguments != null)
    {
      model.Limit = GetArguments(_limit, _limitArguments);
    }

    return model;
  }

  private IEnumerable<ArgumentViewModel> BuildArguments(string localizationPrefix,
    ArgumentDescriptor[] argumentDescriptors, List<ArgumentFlat>? arguments)
  {
    var argumentsMapper = arguments?.ToDictionary(x => x.Name, x => x.Value) ?? new Dictionary<string, string>();

    foreach (var argumentDescriptor in argumentDescriptors)
    {
      var localizationKey = $"{localizationPrefix}.arguments.{argumentDescriptor.Name}";
      var currentValue = argumentsMapper.TryGetValue(argumentDescriptor.Name, out var val) ? val : "";
      yield return new ArgumentViewModel(localizationKey, argumentDescriptor.Name, argumentDescriptor.Type, currentValue);
    }
  }

  private FlatArgumentsSection GetArguments(string? profile, ArgumentViewModel[] profileArguments)
  => new()
  {
    Type = profile,
    Arguments = profileArguments
      .Select(x => new ArgumentFlat()
      {
        Name = x.Name,
        Type = x.Type,
        Value = x.Value,
      }).ToList()
  };

  public void Dispose()
  {
    _subscription.Dispose();
    _scenarioExecutedSubscription.Dispose();
  }
}
