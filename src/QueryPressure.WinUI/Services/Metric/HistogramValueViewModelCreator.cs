using Perfolizer.Mathematics.Histograms;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Theme;
using QueryPressure.WinUI.ViewModels.Execution.Metrics;

namespace QueryPressure.WinUI.Services.Metric;

public class HistogramValueViewModelCreator : IMetricValueViewModelCreator
{
  private readonly IObservableItem<LanguageItem>? _languageObserver;
  private readonly IObservableItem<ApplicationTheme>? _themeObserver;

  public HistogramValueViewModelCreator(IObservableItem<LanguageItem>? languageObserver, IObservableItem<ApplicationTheme>? themeObserver)
  {
    _languageObserver = languageObserver;
    _themeObserver = themeObserver;
  }

  public uint Priority => 1;

  public bool CanFormat(string metricName, object metricValue)
  {
    return metricValue is Histogram;
  }

  public MetricViewModel Create(string contentId, string metricName, string nameLabelKey, object metricValue)
  {
    return new HistogramMetricViewModel(_languageObserver, _themeObserver, contentId, metricName, nameLabelKey);
  }
}
