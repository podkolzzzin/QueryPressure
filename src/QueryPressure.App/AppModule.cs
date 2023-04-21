using System.Globalization;
using Autofac;
using Autofac.Features.AttributeFilters;
using QueryPressure.App.Console;
using QueryPressure.App.Factories;
using QueryPressure.App.Interfaces;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.App;

public class AppModule : Module
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

    builder.RegisterInstance(CultureInfo.CurrentUICulture);

    builder.RegisterType<ConsoleOptions>()
      .SingleInstance();

    builder.RegisterType<DefaultConsoleMetricFormatter>();

    builder.RegisterType<ConsoleMetricFormatterProvider>()
      .As<IConsoleMetricFormatterProvider>()
      .WithAttributeFiltering();

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
