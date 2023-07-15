using QueryPressure.WinUI.Common;

namespace QueryPressure.WinUI.ViewModels.Execution;

public class MetricViewModel : ViewModelBase
{
  private string _nameLabelKey;
  private string _value;

  public MetricViewModel(string name, string value)
  {
    _nameLabelKey = name;
    _value = value;
  }

  public string NameLabelKey
  {
    get => _nameLabelKey;
    set => SetField(ref _nameLabelKey, value);
  }

  public string Value
  {
    get => _value;
    set => SetField(ref _value, value);
  }
}
