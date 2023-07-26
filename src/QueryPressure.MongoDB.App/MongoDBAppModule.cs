using Autofac;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.MongoDB.App;

public class MongoDBAppModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<MongoDBConnectionProviderCreator>()
        .AsImplementedInterfaces();

    builder.RegisterInstance(new MongoProviderInfo())
      .As<IProviderInfo>();
  }
}
