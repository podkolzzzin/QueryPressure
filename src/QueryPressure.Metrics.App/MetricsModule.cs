using Autofac;
using QueryPressure.App.Console;
using QueryPressure.App.Interfaces;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;
using QueryPressure.Metrics.App.Formatters;
using QueryPressure.Metrics.Core;

[assembly: QueryPressurePlugin]

namespace QueryPressure.Metrics.App;

public class MetricsModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<TimeIntervalConsoleMetricFormatter>()
      .As<IConsoleMetricFormatter>();

    builder.RegisterType<ConfidenceIntervalConsoleMetricFormatter>()
      .As<IConsoleMetricFormatter>();

    builder.RegisterType<HistogramConsoleMetricFormatter>()
      .As<IConsoleMetricFormatter>();

    builder.RegisterType<StatisticalMetricsProvider>()
      .As<IMetricProvider>()
      .SingleInstance();

    builder.RegisterType<LiveAverageMetricProvider>()
      .As<ILiveMetricProvider>()
      .InstancePerDependency();

    builder.RegisterType<ThroughputLiveMetricProvider>()
      .As<ILiveMetricProvider>()
      .InstancePerDependency();

    builder.RegisterType<ErrorRateLiveMetricProvider>()
      .As<ILiveMetricProvider>()
      .As<IMetricProvider>()
      .InstancePerDependency();
  }
}
