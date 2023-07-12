using Autofac;
using QueryPressure.App.Arguments;
using QueryPressure.WinUI.Models;
using QueryPressure.WinUI.Services.Subscriptions;
using System.Collections.Concurrent;

namespace QueryPressure.WinUI.Services.Execute;

public record ExecutionData(Execution Execution, Task Task, CancellationTokenSource CancellationTokenSource);

public class ExecutionService : IExecutionService
{
  private readonly ConcurrentDictionary<Guid, ExecutionData> _executions;
  private readonly ILifetimeScope _scope;
  private readonly ISubscriptionManager _subscriptionManager;

  public ExecutionService(ILifetimeScope scope, ISubscriptionManager subscriptionManager)
  {
    _executions = new();
    _scope = scope;
    _subscriptionManager = subscriptionManager;
  }

  public Guid Execute(ScenarioModel parameter)
  {
    var appArgs = CreateArguments(parameter);
    var id = Guid.NewGuid();

    var model = new ExecutionModel(id);
    parameter.Executions.Add(model);

    var executionScope = _scope.BeginLifetimeScope();
    var execution = executionScope.Resolve<Execution>(
      new NamedParameter("args", appArgs),
      new NamedParameter("id", id),
      new NamedParameter("model", model)
    );
    var cancellationTokenSource = new CancellationTokenSource();
    var task = ExecuteAsync(executionScope, execution, cancellationTokenSource.Token);
    var data = new ExecutionData(execution, task, cancellationTokenSource);
    _executions[id] = data;

    _subscriptionManager.Notify(this, ModelAction.ChildrenChanged, parameter);

    return id;
  }

  private async Task ExecuteAsync(ILifetimeScope scope, Execution execution, CancellationToken cancellationToken)
  {
    await using (scope)
    {
      await execution.ExecuteAsync(cancellationToken);
      _executions.TryRemove(execution.Id, out _);
    }
  }

  private static ApplicationArguments CreateArguments(ScenarioModel request)
  {
    var appArgs = new ApplicationArguments()
    {
      ["connection"] = new()
      {
        Type = request.Provider,
        Arguments = new()
        {
          ["connectionString"] = request.ConnectionString!,
        }
      },
      ["profile"] = new()
      {
        Type = request.Profile.Type,
        Arguments = request.Profile.Arguments!.ToDictionary(x => x.Name, x => x.Value),
      },
      ["limit"] = new()
      {
        Type = request.Limit.Type,
        Arguments = request.Limit.Arguments!.ToDictionary(x => x.Name, x => x.Value),
      },
      ["script"] = new()
      {
        Type = "text",
        Arguments = new()
        {
          ["text"] = request.Script!
        }
      }
    };
    return appArgs;
  }
}
