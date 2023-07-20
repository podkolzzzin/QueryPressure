using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Services.Execute;
using QueryPressure.WinUI.Services.Metric;
using QueryPressure.WinUI.ViewModels.Execution.Metrics;

namespace QueryPressure.WinUI.ViewModels.Execution;

public class MetricsViewModel : ViewModelBase
{
  private readonly Dictionary<string, MetricViewModel> _metrics;
  private readonly string _contentId;
  private readonly IMetricViewModelFactory _metricValueViewModelFactory;

  public MetricsViewModel(string contentId, IMetricViewModelFactory metricValueViewModelFactory)
  {
    _metrics = new Dictionary<string, MetricViewModel>();
    _contentId = contentId;
    _metricValueViewModelFactory = metricValueViewModelFactory;
  }

  public MetricType Type { get; private set; }

  public List<MetricViewModel> Metrics => _metrics.Values.ToList();

  public string HeaderLabelKey => Type switch
  {
    MetricType.Realtime => "labels.execution.document.real-time-header",
    MetricType.Result => "labels.execution.document.result-header",
    _ => throw new ArgumentOutOfRangeException(nameof(Type)),
  };

  public void UpdateMetrics(ExecutionVisualization? metrics, MetricType type)
  {
    if (metrics == null) return;

    Type = type;

    foreach (var metric in metrics.Metrics)
    {
      if (_metrics.ContainsKey(metric.Name))
      {
        var viewModel = _metrics[metric.Name];
        viewModel.SetValue(metric.Value);
      }
      else
      {
        _metrics[metric.Name] = _metricValueViewModelFactory.CreateAndSetValue(_contentId, metric.Name, $"metrics.{metric.Name}.title", metric.Value);
      }
    }

    var currentMetrics = metrics.Metrics.Select(x => x.Name).ToHashSet();

    var needToDelete = _metrics.Keys.Where(x => !currentMetrics.Contains(x)).ToList();

    foreach (var metric in needToDelete)
    {
      _metrics.Remove(metric);
    }


    OnOtherPropertyChanged(nameof(Type));
    OnOtherPropertyChanged(nameof(HeaderLabelKey));
    OnOtherPropertyChanged(nameof(Metrics));
  }
}
