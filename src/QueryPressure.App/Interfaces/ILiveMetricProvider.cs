using QueryPressure.Core.Interfaces;

namespace QueryPressure.App.Interfaces;

public interface ILiveMetricProvider : IExecutionHook
{
  IEnumerable<IMetric> GetMetrics();
}
