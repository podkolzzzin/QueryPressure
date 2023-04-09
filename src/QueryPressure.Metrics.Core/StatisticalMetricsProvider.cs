using Perfolizer.Mathematics.Common;
using Perfolizer.Mathematics.Histograms;
using Perfolizer.Mathematics.QuantileEstimators;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Metrics.Core;

public class StatisticalMetricsProvider : IMetricProvider
{

  public Task<IEnumerable<IMetric>> CalculateAsync(IExecutionResultStore store, CancellationToken cancellationToken)
  {
    var sorted = store.OrderBy(x => x.Duration)
      .Select(x => x.Duration.TotalMilliseconds)
      .ToList();

    var quartiles = Quartiles.FromSorted(sorted);
    var moments = Moments.Create(sorted);
    var histogram = BuildSimpleHistogram(sorted, moments.StandardDeviation);

    IEnumerable<IMetric> results = new IMetric[] {
      new SimpleMetric("Q1", TimeSpan.FromMilliseconds(quartiles.Q1)),
      new SimpleMetric("Median", TimeSpan.FromMilliseconds(quartiles.Median)),
      new SimpleMetric("Q3", TimeSpan.FromMilliseconds(quartiles.Q3)),
      new SimpleMetric("StandardDeviation", TimeSpan.FromMilliseconds(moments.StandardDeviation)),
      new SimpleMetric("Mean", TimeSpan.FromMilliseconds(moments.Mean)),
      new SimpleMetric("Histogram", histogram),
    };

    return Task.FromResult(results);
  }

  private static Histogram BuildSimpleHistogram(IReadOnlyList<double> list, double standardDeviation)
  {
    var histogramBinSize = SimpleHistogramBuilder.GetOptimalBinSize(list.Count, standardDeviation);

    if (Math.Abs(histogramBinSize) < 1E-09)
      histogramBinSize = 1.0;

    return HistogramBuilder.Simple.Build(list, histogramBinSize);
  }
}
