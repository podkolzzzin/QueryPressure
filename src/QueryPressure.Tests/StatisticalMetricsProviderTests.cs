using QueryPressure.Core.Interfaces;
using Perfolizer.Mathematics.Distributions;
using QueryPressure.Core;
using QueryPressure.Metrics.Core;
using Xunit;

namespace QueryPressure.Tests;

public class StatisticalMetricsProviderTests
{
  [Fact]
  public async Task Calculate_RandomNormalDistribution_Success()
  {
    const int seed = 42;
    const int count = 1000;
    var expectedMean = TimeSpan.FromSeconds(5);
    var expectedStdDev = TimeSpan.FromSeconds(1);
    var token = CancellationToken.None;

    IMetricProvider metricProvider = new StatisticalMetricsProvider();
    IExecutionResultStore store =
      await GetNormalDistributionStoreAsync(seed, expectedMean, expectedStdDev, count, token);

    var metrics = await metricProvider.CalculateAsync(store, token);

    Assert.NotNull(metrics);
  }

  private static async Task<IExecutionResultStore> GetNormalDistributionStoreAsync(int seed, TimeSpan mean,
    TimeSpan stdDev, int count, CancellationToken token)
  {
    var random = new NormalDistribution(mean.TotalMicroseconds, stdDev.TotalMicroseconds).Random(seed);

    var result = new ExecutionResultStore();

    var executions = random.Next(count)
      .Select(x => new ExecutionResult(DateTime.MinValue, TimeSpan.FromMicroseconds(x), null));

    foreach (var execution in executions)
    {
      await result.OnQueryExecutedAsync(execution, token);
    }

    return result;
  }
}
