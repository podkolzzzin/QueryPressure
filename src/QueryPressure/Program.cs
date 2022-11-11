// See https://aka.ms/new-console-template for more information

using Autofac;
using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;

var loader = new Loader();

var container = loader.Load(args);

var builder = container.Resolve<IScenarioBuilder>();
var appArgs = container.Resolve<ApplicationArguments>();

var executor = await builder.BuildAsync(appArgs, default);

await executor.ExecuteAsync(default);