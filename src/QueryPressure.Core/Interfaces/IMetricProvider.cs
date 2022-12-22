namespace QueryPressure.Core.Interfaces;

public interface IMetricProvider
{
  Task<IEnumerable<IMetric>> CalculateAsync(IExecutionResultStore store, CancellationToken cancellationToken);
}
