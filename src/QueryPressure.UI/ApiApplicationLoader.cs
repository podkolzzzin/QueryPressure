using Autofac;
using QueryPressure.App;
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
    return base.Load(builder);
  }
}
