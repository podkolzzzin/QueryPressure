using Autofac;

namespace QueryPressure.MySql.App;

public class MySqlAppModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<MySqlConnectionProviderCreator>()
      .AsImplementedInterfaces();
  }
}