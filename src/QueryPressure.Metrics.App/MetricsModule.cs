using Autofac;
using Autofac.Features.AttributeFilters;
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
    builder.RegisterType<TimeSpanConsoleMetricFormatter>()
      .As<IConsoleMetricFormatter>()
      .WithAttributeFiltering()
      .SingleInstance();

    builder.RegisterType<HistogramConsoleMetricFormatter>()
      .As<IConsoleMetricFormatter>()
      .SingleInstance();

    builder.RegisterType<StatisticalMetricsProvider>()
      .As<IMetricProvider>()
      .SingleInstance();

    builder.RegisterType<LiveAverageMetricProvider>()
      .As<ILiveMetricProvider>()
      .InstancePerDependency();
  }
}
