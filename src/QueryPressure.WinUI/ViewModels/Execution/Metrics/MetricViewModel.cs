using QueryPressure.WinUI.Common;

namespace QueryPressure.WinUI.ViewModels.Execution.Metrics;

public abstract class MetricViewModel : ViewModelBase, IMetricViewModel
{
  protected MetricViewModel(string nameLabelKey)
  {
    NameLabelKey = nameLabelKey;
  }

  public string NameLabelKey { get; private set; }

  public abstract void SetValue(object value);
}
