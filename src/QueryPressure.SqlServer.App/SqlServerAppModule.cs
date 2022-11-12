using Autofac;

namespace QueryPressure.SqlServer.App
{
  public class SqlServerAppModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<SqlServerConnectionProviderCreator>()
        .AsImplementedInterfaces();
    }
  }
}
