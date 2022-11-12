using Autofac;

namespace QueryPressure.Mongodb.App;

public class MongodbAppModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<MongodbConnectionProviderCreator>()
      .AsImplementedInterfaces();
  }
}