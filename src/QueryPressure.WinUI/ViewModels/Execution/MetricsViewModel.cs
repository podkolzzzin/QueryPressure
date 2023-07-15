using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Services.Execute;

namespace QueryPressure.WinUI.ViewModels.Execution;

public class MetricsViewModel : ViewModelBase
{
  private readonly Dictionary<string, MetricViewModel> _metrics;

  public MetricsViewModel()
  {
    _metrics = new Dictionary<string, MetricViewModel>();
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
        viewModel.NameLabelKey = $"metrics.{metric.Name}.title";
        viewModel.Value = metric.Value.ToString() ?? string.Empty; // TODO: better formating
      }
      else
      {
        var viewModel = new MetricViewModel($"metrics.{metric.Name}.title", metric.Value.ToString() ?? string.Empty);
        _metrics[metric.Name] = viewModel;
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
