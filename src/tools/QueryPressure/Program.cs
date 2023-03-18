// See https://aka.ms/new-console-template for more information

using Autofac;
using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;

var token = CancellationToken.None;
var loader = new ConsoleApplicationLoader(args);

var container = loader.Load(new ContainerBuilder()).Build();

var builder = container.Resolve<IScenarioBuilder>();
var appArgs = container.Resolve<ApplicationArguments>();

var store = container.Resolve<IExecutionResultStore>();
var liveMetrics = container.Resolve<ILiveMetricProvider[]>();
var executor = await builder.BuildAsync(appArgs, store, liveMetrics, token);

var visualizer = container.ResolveKeyed<IMetricsVisualizer>("Console");
var executionTask = executor.ExecuteAsync(token);
while (!executionTask.IsCompleted)
{
  await Task.WhenAny(executionTask, Task.Delay(200));
  var liveVisualization = await visualizer.VisualizeAsync(liveMetrics.SelectMany(x => x.GetMetrics()), token);
  Console.Clear();
  Console.WriteLine(liveVisualization);
}

Console.WriteLine("=======================");

var calculator = container.Resolve<IMetricsCalculator>();
var metrics = await calculator.CalculateAsync(store, token); // IEnumerable<IMetric>


var visualization = await visualizer.VisualizeAsync(metrics, token);

Console.WriteLine(visualization);
