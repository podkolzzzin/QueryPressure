using System.Collections.Immutable;
using System.Diagnostics;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core;

public class QueryExecutor
{
  private readonly IExecutable _executable;
  private readonly IProfile _loadProfile;
  private readonly ILimit _limit;
  private readonly ImmutableArray<IExecutionHook> _hooks;

  public QueryExecutor(IExecutable executable, IProfile loadProfile, ILimit limit, IExecutionResultStore store, IEnumerable<IExecutionHook> otherHooks)
  {
    _executable = executable;
    _loadProfile = loadProfile;
    _limit = limit;

    var hooks = ImmutableArray.CreateBuilder<IExecutionHook>();

    if (loadProfile is IExecutionHook hookProfile)
      hooks.Add(hookProfile);
    if (limit is IExecutionHook hookLimit)
      hooks.Add(hookLimit);

    hooks.Add(store);
    hooks.AddRange(otherHooks);

    _hooks = hooks.ToImmutable();
  }

  public async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _limit.Token);
    var token = source.Token;
    try
    {
      while (!token.IsCancellationRequested)
      {
        await _loadProfile.WhenNextCanBeExecutedAsync(token);

        if (token.IsCancellationRequested)
        {
          break;
        }

        var info = new QueryInformation(Guid.NewGuid(), DateTime.UtcNow);
        var stopwatch = Stopwatch.StartNew();
        
        // Don't await this task, we want to start the execution as soon as possible
        var __ = Task.WhenAll(_hooks.Select(x => x.OnBeforeQueryExecutionAsync(info, token)));
        var _ = _executable.ExecuteAsync(token).ContinueWith(async executionTask =>
        {
          if (token.IsCancellationRequested)
            return;

          stopwatch.Stop();
          var result = new ExecutionResult(info, stopwatch.Elapsed, executionTask.Exception);
          await Task.WhenAll(_hooks.Select(x => x.OnQueryExecutedAsync(result, token)));
        }, token);
      }
    }
    catch (OperationCanceledException) { }
  }
}
