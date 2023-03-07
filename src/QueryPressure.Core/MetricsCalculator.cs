using System.Collections.Immutable;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core;

public class MetricsCalculator : IMetricsCalculator
{
  private readonly IEnumerable<IMetricProvider> _providers;
  public MetricsCalculator(IEnumerable<IMetricProvider> providers)
  {
    _providers = providers;
  }
  public async Task<IEnumerable<IMetric>> CalculateAsync(IExecutionResultStore store, CancellationToken cancellationToken)
  {
    var result = ImmutableArray.CreateBuilder<IMetric>();
    foreach (var provider in _providers)
    {
      var metrics = await provider.CalculateAsync(store, cancellationToken);
      result.AddRange(metrics);
    }

    return result;
  }
}
