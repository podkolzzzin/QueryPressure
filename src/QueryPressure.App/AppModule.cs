using Autofac;
using QueryPressure.App.Factories;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.App;

public class AppModule : Autofac.Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterAssemblyTypes(ThisAssembly)
      .Where(t => t.Name.EndsWith("Creator"))
      .AsImplementedInterfaces();
    
    builder.RegisterType<SettingsFactory<IProfile>>()
      .As<ISettingsFactory<IProfile>>()
      .WithParameter("settingType", "profile");
        
    builder.RegisterType<SettingsFactory<ILimit>>()
      .As<ISettingsFactory<ILimit>>()
      .WithParameter("settingType", "limit");
        
    builder.RegisterType<SettingsFactory<IConnectionProvider>>()
      .As<ISettingsFactory<IConnectionProvider>>()
      .WithParameter("settingType", "connection");
        
    builder.RegisterType<SettingsFactory<IScriptSource>>()
      .As<ISettingsFactory<IScriptSource>>()
      .WithParameter("settingType", "script");

    builder.RegisterType<ScenarioBuilder>()
      .As<IScenarioBuilder>();
  }
}