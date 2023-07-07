using QueryPressure.WinUI.Common;

namespace QueryPressure.WinUI.ViewModels.Properties
{
  public class ArgumentViewModel : ViewModelBase
  {
    public ArgumentViewModel(string localizationKey, string name, string type, string value)
    {
      LocalizationKey = localizationKey;
      Name = name;
      Type = type;
      Value = value;
    }

    public string LocalizationKey { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
  }
}
