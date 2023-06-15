using System.Collections.Concurrent;
using Autofac;
using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.UI;

public class Provider
{
  private record ExecutionData(Execution Execution, Task Task, CancellationTokenSource CancellationTokenSource);

  private readonly ILifetimeScope _scope;
  private readonly ICreator<IConnectionProvider> _creator;
  private readonly ConcurrentDictionary<Guid, ExecutionData> _executions = new();

  public Provider(ILifetimeScope scope, ICreator<IConnectionProvider> creator)
  {
    _scope = scope;
    _creator = creator;
  }

  public async Task<IServerInfo> TestConnectionAsync(string connectionString)
  {
    var provider = _creator.Create(new ArgumentsSection()
    {
      Arguments = new Dictionary<string, string>()
      {
        ["connectionString"] = connectionString // TODO: put constant connectionString somewhere
      }
    });
    return await provider.GetServerInfoAsync(default);
  }

  public Guid StartExecution(ExecutionRequest request)
  {
    var appArgs = CreateArguments(request);
    var id = Guid.NewGuid();
    var executionScope = _scope.BeginLifetimeScope();
    var execution = executionScope.Resolve<Execution>(
      new NamedParameter("args", appArgs),
      new NamedParameter("id", id)
    );
    var cancellationTokenSource = new CancellationTokenSource();
    var task = ExecuteAsync(executionScope, execution, cancellationTokenSource.Token);
    var data = new ExecutionData(execution, task, cancellationTokenSource);
    _executions[id] = data;

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

  private static ApplicationArguments CreateArguments(ExecutionRequest request)
  {
    var appArgs = new ApplicationArguments()
    {
      ["connection"] = new()
      {
        Type = request.Provider,
        Arguments = new()
        {
          ["connectionString"] = request.ConnectionString,
        }
      },
      ["profile"] = new()
      {
        Type = request.Profile.Type,
        Arguments = request.Profile.Arguments.ToDictionary(x => x.Name, x => x.Value),
      },
      ["limit"] = new()
      {
        Type = request.Limit.Type,
        Arguments = request.Limit.Arguments.ToDictionary(x => x.Name, x => x.Value),
      },
      ["script"] = new()
      {
        Type = "text",
        Arguments = new()
        {
          ["text"] = request.Script
        }
      }
    };
    return appArgs;
  }
}
