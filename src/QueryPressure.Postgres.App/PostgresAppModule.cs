using Autofac;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Postgres.App;

public class PostgresAppModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<PostgresConnectionProviderCreator>()
      .AsImplementedInterfaces();

    builder.RegisterType<PostgresProviderInfo>()
      .As<IProviderInfo>()
      .SingleInstance();
  }
}
