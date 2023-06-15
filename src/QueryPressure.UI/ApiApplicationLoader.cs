using Autofac;
using Autofac.Features.AttributeFilters;
using QueryPressure.App;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.UI;

public class ApiApplicationLoader : ApplicationLoader
{
  public override ContainerBuilder Load(ContainerBuilder builder)
  {
    builder.RegisterType<ProviderManager>()
      .AsSelf()
      .SingleInstance();

    builder.RegisterType<Provider>()
      .AsSelf();

    builder.RegisterType<Launcher>()
      .AsSelf()
      .SingleInstance();

    builder.RegisterGeneric(typeof(HubService<>))
      .AsImplementedInterfaces()
      .SingleInstance();

    builder.RegisterType<DashboardVisualizer>()
      .Keyed<IMetricsVisualizer>(DashboardVisualizer.Key)
      .SingleInstance();

    builder.RegisterType<Execution>()
      .AsSelf()
      .InstancePerLifetimeScope()
      .WithAttributeFiltering();

    return base.Load(builder);
  }
}
