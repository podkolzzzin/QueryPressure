using QueryPressure.WinUI.Common;

namespace QueryPressure.WinUI.ViewModels.Properties
{
  public class ArgumentViewModel : ViewModelBase
  {
    private readonly Action<string, string> _valueArgumentValueEdited;
    private string _value;

    public ArgumentViewModel(string localizationKey, string name, string type, string value, Action<string, string> valueArgumentValueEdited)
    {
      LocalizationKey = localizationKey;
      Name = name;
      Type = type;
      _value = value;
      _valueArgumentValueEdited = valueArgumentValueEdited;
    }

    public string LocalizationKey { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }

    public string Value
    {
      get => _value;
      set
      {
        SetField(ref _value, value);
        _valueArgumentValueEdited.Invoke(Name, _value);
      }
    }

    public void SetArgumentValue(string value)
    {
      _value = value;
      OnOtherPropertyChanged(nameof(Value));
    }
  }
}
