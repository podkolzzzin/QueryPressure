using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Metrics.Core;

namespace QueryPressure.Metrics.App;

public class ErrorRateLiveMetricProvider : ILiveMetricProvider, IMetricProvider
{
  private readonly SlidingWindowRateCounter
    _errorRate = new(TimeSpan.FromSeconds(1));

  private int _errorCount;

  public Task OnQueryExecutedAsync(ExecutionResult result, CancellationToken cancellationToken)
  {
    if (result.Exception != null)
    {
      _errorRate.RegisterCall();
      Interlocked.Increment(ref _errorCount);
    }
    return Task.CompletedTask;
  }

  public IEnumerable<IMetric> GetMetrics()
  {
    yield return new SimpleMetric("live-error-rate", _errorRate.GetCallsPerTimeWindow());
    yield return new SimpleMetric("live-error-count", _errorCount);
  }
  
  public Task<IEnumerable<IMetric>> CalculateAsync(IExecutionResultStore store, CancellationToken cancellationToken)
  {
    var result = new[] { new SimpleMetric("live-error-count", _errorCount) };
    return Task.FromResult<IEnumerable<IMetric>>(result);
  }
}
