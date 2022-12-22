using Perfolizer.Mathematics.Common;
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

    IEnumerable<IMetric> results = new IMetric[] {
      new SimpleMetric("Q1", TimeSpan.FromMilliseconds(quartiles.Q1)),
      new SimpleMetric("Median", TimeSpan.FromMilliseconds(quartiles.Median)),
      new SimpleMetric("Q3", TimeSpan.FromMilliseconds(quartiles.Q3)),
      new SimpleMetric("StandardDeviation", TimeSpan.FromMilliseconds(moments.StandardDeviation)),
      new SimpleMetric("Mean", TimeSpan.FromMilliseconds(moments.Mean))
    };

    return Task.FromResult(results);
  }
}
