using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Metrics.Core;

namespace QueryPressure.Metrics.App;

public class LiveAverageMetricProvider : ILiveMetricProvider
{
  private int _count;
  private long _ticks;

  public Task OnQueryExecutedAsync(ExecutionResult result, CancellationToken cancellationToken)
  {
    Interlocked.Add(ref _ticks, result.Duration.Ticks);
    Interlocked.Increment(ref _count);
    return Task.CompletedTask;
  }
  public IEnumerable<IMetric> GetMetrics()
  {
    yield return new SimpleMetric("Average", _count == 0 ? TimeSpan.Zero : new TimeSpan(_ticks / _count));
    yield return new SimpleMetric("Request Count", _count);
  }
}
