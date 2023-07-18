using Perfolizer.Horology;
using Perfolizer.Mathematics.Common;
using Perfolizer.Mathematics.Histograms;
using Perfolizer.Mathematics.QuantileEstimators;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Metrics.Core;

public class StatisticalMetricsProvider : IMetricProvider
{

  public Task<IEnumerable<IMetric>> CalculateAsync(IEnumerable<ExecutionResult> store, CancellationToken cancellationToken)
  {
    var sorted = store.OrderBy(x => x.Duration)
      .Select(x => x.Duration.TotalNanoseconds)
      .ToList();

    var quartiles = Quartiles.FromSorted(sorted);
    var moments = Moments.Create(sorted);
    var standardError = moments.StandardDeviation / Math.Sqrt(sorted.Count);
    var confidenceInterval = new ConfidenceInterval(moments.Mean, standardError, sorted.Count);
    var histogram = BuildSimpleHistogram(sorted, moments.StandardDeviation);

    IEnumerable<IMetric> results = new IMetric[] {
      new SimpleMetric("min", TimeInterval.FromNanoseconds(quartiles.Min)),
      new SimpleMetric("q1", TimeInterval.FromNanoseconds(quartiles.Q1)),
      new SimpleMetric("median", TimeInterval.FromNanoseconds(quartiles.Median)),
      new SimpleMetric("q3", TimeInterval.FromNanoseconds(quartiles.Q3)),
      new SimpleMetric("max", TimeInterval.FromNanoseconds(quartiles.Max)),
      new SimpleMetric("mean", TimeInterval.FromNanoseconds(moments.Mean)),
      new SimpleMetric("standard-deviation", TimeInterval.FromNanoseconds(moments.StandardDeviation)),
      new SimpleMetric("standard-error", TimeInterval.FromNanoseconds(standardError)),
      new SimpleMetric("confidence-interval", confidenceInterval),
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
