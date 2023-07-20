using QueryPressure.App.Console;

namespace QueryPressure.WinUI.ViewModels.Execution.Metrics;

public class DefaultMetricViewModel : MetricViewModel
{
  private readonly string _metricName;
  private readonly IConsoleMetricFormatter _formatProvider;

  private string _value = string.Empty;

  public DefaultMetricViewModel(string metricName, string nameLabelKey, IConsoleMetricFormatter formatProvider) : base(nameLabelKey)
  {
    _metricName = metricName;
    _formatProvider = formatProvider;
  }

  public string Value
  {
    get => _value;
    set => SetField(ref _value, value);
  }

  public override void SetValue(object value)
  {
    if (_formatProvider is IConsoleMetricRowFormatter rowFormatter)
    {
      Value = rowFormatter.GetValue(_metricName, value);
    }
    else
    {
      Value = _formatProvider.Format(_metricName, value);
    }
  }
}
