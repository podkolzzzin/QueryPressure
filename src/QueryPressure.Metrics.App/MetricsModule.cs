using Autofac;
using QueryPressure.App.Interfaces;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;
using QueryPressure.Metrics.Core;

[assembly: QueryPressurePlugin]

namespace QueryPressure.Metrics.App;

public class MetricsModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<StatisticalMetricsProvider>()
      .As<IMetricProvider>()
      .SingleInstance();

    builder.RegisterType<LiveAverageMetricProvider>()
      .As<ILiveMetricProvider>()
      .InstancePerDependency();
  }
}
