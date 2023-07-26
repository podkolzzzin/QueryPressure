using Autofac;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Postgres.App;

public class PostgresAppModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<PostgresConnectionProviderCreator>()
      .AsImplementedInterfaces();

    builder.RegisterInstance(new PostgresProviderInfo())
      .As<IProviderInfo>();
  }
}
