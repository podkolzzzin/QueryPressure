using QueryPressure.App.Console;
using QueryPressure.WinUI.ViewModels.Execution.Metrics;

namespace QueryPressure.WinUI.Services.Metric;

public class DefaultMetricValueViewModelCreator : IMetricValueViewModelCreator
{
  private readonly IConsoleMetricFormatterProvider _consoleMetricFormatterProvider;

  public DefaultMetricValueViewModelCreator(IConsoleMetricFormatterProvider consoleMetricFormatterProvider)
  {
    _consoleMetricFormatterProvider = consoleMetricFormatterProvider;
  }

  public uint Priority => 0;

  public bool CanFormat(string metricName, object metricValue) => true;

  public MetricViewModel Create(string contentId, string metricName, string nameLabelKey, object metricValue)
  {
    var formatProvider = _consoleMetricFormatterProvider.Get(metricName, metricValue);
    return new DefaultMetricViewModel(metricName, nameLabelKey, formatProvider);
  }
}
