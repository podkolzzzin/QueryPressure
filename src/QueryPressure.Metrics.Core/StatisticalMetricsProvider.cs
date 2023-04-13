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
      new SimpleMetric("q1", TimeSpan.FromMilliseconds(quartiles.Q1)),
      new SimpleMetric("median", TimeSpan.FromMilliseconds(quartiles.Median)),
      new SimpleMetric("q3", TimeSpan.FromMilliseconds(quartiles.Q3)),
      new SimpleMetric("standard-deviation", TimeSpan.FromMilliseconds(moments.StandardDeviation)),
      new SimpleMetric("mean", TimeSpan.FromMilliseconds(moments.Mean)),
      new SimpleMetric("histogram", histogram),
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
