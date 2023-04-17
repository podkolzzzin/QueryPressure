using Perfolizer.Horology;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Metrics.Core;

namespace QueryPressure.Metrics.App;

public class LiveAverageMetricProvider : ILiveMetricProvider
{
  private int _count;
  private long _totalNanoseconds;

  public Task OnQueryExecutedAsync(ExecutionResult result, CancellationToken cancellationToken)
  {
    Interlocked.Add(ref _totalNanoseconds, (long)result.Duration.TotalNanoseconds);
    Interlocked.Increment(ref _count);
    return Task.CompletedTask;
  }
  public IEnumerable<IMetric> GetMetrics()
  {
    yield return new SimpleMetric("live-average",
      _count == 0
        ? TimeInterval.FromNanoseconds(0)
        : TimeInterval.FromNanoseconds((double)_totalNanoseconds / _count));
    yield return new SimpleMetric("live-request-count", _count);
  }
}
