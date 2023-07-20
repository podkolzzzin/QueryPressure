using QueryPressure.WinUI.ViewModels.Execution.Metrics;

namespace QueryPressure.WinUI.Services.Metric;

public interface IMetricViewModelFactory
{
  MetricViewModel CreateAndSetValue(string contentId, string metricName, string nameLabelKey, object metricValue);
}

public class MetricValueViewModelFactory : IMetricViewModelFactory
{
  private readonly IReadOnlyList<IMetricValueViewModelCreator> _customMetricValueViewModelCreators;
  private readonly IMetricValueViewModelCreator _defaultCreator;

  public MetricValueViewModelFactory(IEnumerable<IMetricValueViewModelCreator> customMetricCreators,
    DefaultMetricValueViewModelCreator defaultCreator)
  {
    _customMetricValueViewModelCreators = customMetricCreators.OrderByDescending(x => x.Priority).ToList();
    _defaultCreator = defaultCreator;
  }

  public MetricViewModel CreateAndSetValue(string contentId, string metricName, string nameLabelKey, object metricValue)
  {
    var viewModel = CreateViewModel(contentId, metricName, nameLabelKey, metricValue);
    viewModel.SetValue(metricValue);
    return viewModel;
  }

  private MetricViewModel CreateViewModel(string contentId, string metricName, string nameLabelKey, object metricValue)
  {
    var metricViewModelCreator = GetCreator(metricName, metricValue);
    return metricViewModelCreator.Create(contentId, metricName, nameLabelKey, metricValue);
  }

  private IMetricValueViewModelCreator GetCreator(string metricName, object metricValue)
  {
    var customCreator = _customMetricValueViewModelCreators
      .FirstOrDefault(x => x.CanFormat(metricName, metricValue));

    return customCreator ?? _defaultCreator;
  }
}
