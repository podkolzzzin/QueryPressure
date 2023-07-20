using Perfolizer.Mathematics.Histograms;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.ViewModels.Execution.Metrics;

namespace QueryPressure.WinUI.Services.Metric;

public class HistogramValueViewModelCreator : IMetricValueViewModelCreator
{
  private readonly IObservableItem<LanguageItem>? _languageObserver;

  public HistogramValueViewModelCreator(IObservableItem<LanguageItem>? languageObserver)
  {
    _languageObserver = languageObserver;
  }

  public uint Priority => 1;

  public bool CanFormat(string metricName, object metricValue)
  {
    return metricValue is Histogram;
  }

  public MetricViewModel Create(string contentId, string metricName, string nameLabelKey, object metricValue)
  {
    return new HistogramMetricViewModel(_languageObserver, contentId, metricName, nameLabelKey);
  }
}
