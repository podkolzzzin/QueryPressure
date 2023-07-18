namespace QueryPressure.Core.Interfaces;

public interface IMetricProvider
{
  Task<IEnumerable<IMetric>> CalculateAsync(IEnumerable<ExecutionResult> store, CancellationToken cancellationToken);
}
