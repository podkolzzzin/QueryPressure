using Autofac;

namespace QueryPressure.MongoDB.App;

public class MongoDBAppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MongoDBConnectionProviderCreator>()
            .AsImplementedInterfaces();
    }
}