using Autofac;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.MySql.App;

public class MySqlAppModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<MySqlConnectionProviderCreator>()
      .AsImplementedInterfaces();

    builder.RegisterInstance(new MySqlProviderInfo())
      .As<IProviderInfo>();
  }
}
