namespace QueryPressure.Core.Interfaces;

public interface IMetricsCalculator
{
  Task<IEnumerable<IMetric>> CalculateAsync(IEnumerable<ExecutionResult> store, CancellationToken cancellationToken);
}

public interface IMetric
{
  string Name { get; }
  object Value { get; }
}

public interface IMetricsVisualizer
{
  Task<IVisualization> VisualizeAsync(IEnumerable<IMetric> metrics, CancellationToken cancellationToken);
}

public interface IVisualization
{

}
