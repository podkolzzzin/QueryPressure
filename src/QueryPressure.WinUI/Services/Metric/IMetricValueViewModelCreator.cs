using QueryPressure.WinUI.ViewModels.Execution.Metrics;

namespace QueryPressure.WinUI.Services.Metric;

public interface IMetricValueViewModelCreator
{
  uint Priority { get; }

  bool CanFormat(string metricName, object metricValue);

  MetricViewModel Create(string contentId, string metricName, string nameLabelKey, object metricValue);
}
