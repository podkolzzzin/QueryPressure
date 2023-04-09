using Autofac;
using QueryPressure.App.Console;
using QueryPressure.App.Factories;
using QueryPressure.App.Interfaces;
using QueryPressure.Core;
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

    builder.RegisterType<ExecutionResultStore>()
      .As<IExecutionResultStore>();

    builder.RegisterType<MetricsCalculator>()
      .As<IMetricsCalculator>();

    builder.RegisterType<DefaultConsoleMetricFormatter>()
      .Keyed<IConsoleMetricFormatter>("default");

    builder.Register(ctx =>
    {
      var defaultFormatter = ctx.ResolveKeyed<IConsoleMetricFormatter>("default");
      var services = ctx.Resolve<IEnumerable<IConsoleMetricFormatter>>()
        .Except(new[] { defaultFormatter }).ToList();

      return new ConsoleMetricFormatterProvider(services, defaultFormatter);
    }).As<IConsoleMetricFormatterProvider>();

    builder.RegisterType<ConsoleMetricsVisualizer>()
      .Keyed<IMetricsVisualizer>("Console");

    builder.RegisterType<ResourceManager>()
      .As<IResourceManager>()
      .SingleInstance();

    builder.RegisterType<EmbeddedResourceDiscovery>()
      .As<IResourceDiscovery>()
      .SingleInstance();
  }
}
