using QueryPressure.App.Arguments;
using QueryPressure.WinUI.Common;

namespace QueryPressure.WinUI.ViewModels.Properties;

public class ArgumentsPropertiesViewModel : ViewModelBase
{
  private readonly Dictionary<string, ArgumentDescriptor[]> _argumentsMapper;
  private readonly string _localizationPrefix;
  private readonly Action<SetModelType> _notifyArgumentsChanged;
  private KeyValuePair<string, string> _currentType;
  private string? _currentArgumentsForType;
  private ArgumentViewModel[]? _arguments;

  public ArgumentsPropertiesViewModel(IEnumerable<string> types,
    Dictionary<string, ArgumentDescriptor[]> argumentsMapper, string localizationPrefix, Action<SetModelType> notifyArgumentsChanged)
  {
    _localizationPrefix = localizationPrefix;
    _argumentsMapper = argumentsMapper;
    _notifyArgumentsChanged = notifyArgumentsChanged;

    Types = types.Select(GetType).ToArray();
  }

  public KeyValuePair<string, string>[] Types { get; set; }

  public KeyValuePair<string, string> CurrentType
  {
    get => _currentType;
    set
    {
      if (SetField(ref _currentType, value))
      {
        Arguments = BuildArguments(_currentType.Key, null).ToArray();
        _notifyArgumentsChanged.Invoke(SetModelType.Direct);
      }
    }
  }

  public ArgumentViewModel[]? Arguments
  {
    get => _arguments;
    set => SetField(ref _arguments, value);
  }

  public void SetValue(FlatArgumentsSection argumentsSection)
  {
    CurrentType = GetType(argumentsSection.Type!);

    if (_currentType.Key != _currentArgumentsForType || _arguments is null)
    {
      Arguments = BuildArguments(_currentType.Key, argumentsSection.Arguments).ToArray();
      _currentArgumentsForType = _currentType.Key;
    }
    else
    {
      SetArguments(argumentsSection.Arguments!);
    }
  }

  private void SetArguments(List<ArgumentFlat> arguments)
  {
    var argumentViewModelProvider = _arguments!.ToDictionary(x => x.Name);

    foreach (var argument in arguments)
    {
      var viewModel = argumentViewModelProvider.TryGetValue(argument.Name, out var argValue)
        ? argValue
        : throw new InvalidOperationException();

      viewModel.Value = argument.Value;
    }
  }

  private IEnumerable<ArgumentViewModel> BuildArguments(string type, List<ArgumentFlat>? arguments)
  {
    var argumentDescriptors = _argumentsMapper[type];
    var argumentsMapper = arguments?.ToDictionary(x => x.Name, x => x.Value) ?? new Dictionary<string, string>();

    foreach (var argumentDescriptor in argumentDescriptors)
    {
      var localizationKey = $"{_localizationPrefix}.{type}.arguments.{argumentDescriptor.Name}";
      var currentValue = argumentsMapper.TryGetValue(argumentDescriptor.Name, out var val) ? val : "";
      yield return new ArgumentViewModel(localizationKey, argumentDescriptor.Name, argumentDescriptor.Type,
        currentValue, (name, value) => _notifyArgumentsChanged.Invoke(SetModelType.Debounced));
    }
  }

  public FlatArgumentsSection GetArguments()
    => new()
    {
      Type = _currentType.Key,
      Arguments = _arguments!
      .Select(x => new ArgumentFlat()
      {
        Name = x.Name,
        Type = x.Type,
        Value = x.Value,
      }).ToList()
    };

  private KeyValuePair<string, string> GetType(string x)
  => KeyValuePair.Create(x, $"{_localizationPrefix}.{x}");
}
