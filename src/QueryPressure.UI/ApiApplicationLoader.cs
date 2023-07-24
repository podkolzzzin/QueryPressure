using Autofac;
using QueryPressure.App;
using QueryPressure.UI.HostedServices;
using QueryPressure.UI.Services;

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
    
    builder.RegisterType<ExecutionStore>()
      .AsImplementedInterfaces()
      .SingleInstance();

    builder.RegisterType<Launcher>()
      .AsSelf()
      .SingleInstance();

    builder.RegisterGeneric(typeof(HubService<>))
      .AsImplementedInterfaces()
      .SingleInstance();

    builder.RegisterType<ExecutionEventPublisher>()
      .SingleInstance();
    
    builder.RegisterType<ExecutionStatusWatcher>()
      .As<IHostedService>()
      .SingleInstance();
    
    builder.RegisterType<ExecutionFinalizer>()
      .As<IHostedService>()
      .SingleInstance();

    return base.Load(builder);
  }
}