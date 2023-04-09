using Autofac;
using QueryPressure.App;

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

    return base.Load(builder);
  }
}
