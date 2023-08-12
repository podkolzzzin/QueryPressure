using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Metrics.Core;

namespace QueryPressure.Metrics.App;

public class ThroughputLiveMetricProvider : ILiveMetricProvider
{
  private readonly SlidingWindowRateCounter
    _started = new(TimeSpan.FromSeconds(1)),
    _finished = new(TimeSpan.FromSeconds(1));

  public Task OnQueryExecutedAsync(ExecutionResult result, CancellationToken cancellationToken)
  {
    _finished.RegisterCall();
    return Task.CompletedTask;
  }

  public Task OnBeforeQueryExecutionAsync(QueryInformation _, CancellationToken __)
  {
    _started.RegisterCall();
    return Task.CompletedTask;
  }

  public IEnumerable<IMetric> GetMetrics()
  {
    yield return new SimpleMetric("live-throughput-handled", _finished.GetCallsPerTimeWindow());
    yield return new SimpleMetric("live-throughput-sent", _started.GetCallsPerTimeWindow());
  }
}
