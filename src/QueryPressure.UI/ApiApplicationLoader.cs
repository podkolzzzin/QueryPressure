using Autofac;
using QueryPressure.App;
using QueryPressure.Core.Interfaces;
using QueryPressure.UI;

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

    return base.Load(builder);
  }
}
