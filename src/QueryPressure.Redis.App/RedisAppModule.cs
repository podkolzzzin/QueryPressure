using Autofac;

namespace QueryPressure.Redis.App;

public sealed class RedisAppModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<RedisConnectionProviderCreator>()
      .AsImplementedInterfaces();
  }
}
