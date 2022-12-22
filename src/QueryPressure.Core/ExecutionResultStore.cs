using System.Collections;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core;

public class ExecutionResultStore : IExecutionResultStore
{
  private readonly LinkedList<ExecutionResult> _results = new();
  
  public Task OnQueryExecutedAsync(ExecutionResult result, CancellationToken cancellationToken)
  {
    _results.AddLast(result);
    return Task.CompletedTask;
  }
  public IEnumerator<ExecutionResult> GetEnumerator()
  {
    return _results.GetEnumerator();
  }
  
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}
