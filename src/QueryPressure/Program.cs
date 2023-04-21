using Autofac;
using QueryPressure;
using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

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
var clearBuffer = string.Empty;

while (!executionTask.IsCompleted)
{
  await Task.WhenAny(executionTask, Task.Delay(200));
  var liveVisualization = await visualizer.VisualizeAsync(liveMetrics.SelectMany(x => x.GetMetrics()), token);
  UpdateLiveTable(liveVisualization);
}

var calculator = container.Resolve<IMetricsCalculator>();
var metrics = await calculator.CalculateAsync(store, token); // IEnumerable<IMetric>

var visualization = await visualizer.VisualizeAsync(metrics, token);

PrintResult(visualization);

void PrintResult(IVisualization result)
{
  var oldColor = Console.ForegroundColor;

  Console.ForegroundColor = ConsoleColor.Cyan;
  Console.WriteLine(result);
  Console.ForegroundColor = oldColor;
}

void UpdateLiveTable(IVisualization live)
{
  var oldColor = Console.ForegroundColor;

  Console.ForegroundColor = ConsoleColor.Yellow;

  if (string.IsNullOrEmpty(clearBuffer))
  {
    Console.Clear();
    Console.Write(live);

    var line = string.Empty.PadLeft(Console.WindowWidth, ' ');
    clearBuffer = Enumerable.Range(0, Console.CursorTop)
      .Aggregate(new StringBuilder(), (stringBuilder, _) => stringBuilder.AppendLine(line))
      .ToString();
  }
  else
  {
    Console.SetCursorPosition(0, 0);
    Console.Write(clearBuffer);
    Console.SetCursorPosition(0, 0);
    Console.Write(live);
  }

  Console.ForegroundColor = oldColor;
}
