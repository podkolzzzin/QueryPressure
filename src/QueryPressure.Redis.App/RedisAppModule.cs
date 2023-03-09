using Autofac;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Redis.App;

public sealed class RedisAppModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<RedisConnectionProviderCreator>()
      .AsImplementedInterfaces();

    builder.RegisterInstance(new ProviderInfo("Redis"))
      .As<IProviderInfo>();
  }
}
