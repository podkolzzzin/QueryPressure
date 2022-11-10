using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.Limits;

public class QueryCountLimit : ILimit, IExecutionHook
{
  private readonly int _count;
  private readonly CancellationTokenSource _source;

  private int _currentCount;
  
  public QueryCountLimit(int count)
  {
    _count = count;
    _source = new();
  }
  
  public CancellationToken Token => _source.Token;
  
  public Task OnQueryExecutedAsync(ExecutionResult _, CancellationToken cancellationToken)
  {
    _currentCount++;
    if (_currentCount > _count)
    {
      _source.Cancel();
    }
    
    return Task.CompletedTask;
  }
}