using QueryPressure.Core.Interfaces;

namespace QueryPressure.Metrics.Core;

public class AverageMetricProvider : IMetricProvider
{
  public Task<IEnumerable<IMetric>> CalculateAsync(IExecutionResultStore store, CancellationToken cancellationToken)
  {
    var result = store.Average(x => x.Duration.TotalMilliseconds);
    return Task.FromResult<IEnumerable<IMetric>>(new IMetric[] {
      new SimpleMetric("Average", TimeSpan.FromMilliseconds(result))
    });
  }
}
