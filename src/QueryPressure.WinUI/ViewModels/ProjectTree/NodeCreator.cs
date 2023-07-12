using Microsoft.Extensions.DependencyInjection;
using QueryPressure.WinUI.Commands.Execution;
using QueryPressure.WinUI.Commands.Scenario;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;

public class NodeCreator : INodeCreator
{
  private readonly IServiceProvider _serviceProvider;

  public NodeCreator(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  public BaseNodeViewModel Create<T>(T model) where T : IModel
    => model switch
    {
      ProjectModel project => CreateProject(project),
      ScenarioModel scenario => CreateScenario(scenario),
      ExecutionModel execution => CreateExecution(execution),
      _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
    };

  private ProjectNodeViewModel CreateProject(ProjectModel project)
    => new(_serviceProvider.GetRequiredService<ISubscriptionManager>(),
      _serviceProvider.GetRequiredService<INodeCreator>(), project);

  private ScenarioNodeViewModel CreateScenario(ScenarioModel project)
    => new(_serviceProvider.GetRequiredService<ISubscriptionManager>(),
      _serviceProvider.GetRequiredService<OpenScenarioScriptCommand>(),
      _serviceProvider.GetRequiredService<INodeCreator>(),
      project);

  private ExecutionNodeViewModel CreateExecution(ExecutionModel execution)
  => new(_serviceProvider.GetRequiredService<ISubscriptionManager>(),
    _serviceProvider.GetRequiredService<OpenExecutionResultsCommand>(), execution);
}
