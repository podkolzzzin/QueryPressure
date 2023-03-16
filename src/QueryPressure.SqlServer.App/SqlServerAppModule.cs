using Autofac;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.SqlServer.App
{
  public class SqlServerAppModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<SqlServerConnectionProviderCreator>()
        .AsImplementedInterfaces();

      builder.RegisterInstance(new ProviderInfo("SqlServer"))
        .As<IProviderInfo>();
    }
  }
}
