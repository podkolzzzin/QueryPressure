using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.Limits
{
  public sealed class TillNErrorsLimit : ILimit, IExecutionHook
  {
    private readonly CancellationTokenSource _source;
    private readonly int _errorLimit;
    private int _errorCount;

    public TillNErrorsLimit(int errorLimit)
    {
      if (errorLimit <= 0)
      {
        throw new ArgumentOutOfRangeException(
          nameof(errorLimit),
          $"{nameof(errorLimit)} parameter must be greater than zero. Actual value: {errorLimit}");
      }

      _source = new();
      _errorLimit = errorLimit;
      _errorCount = 0;
    }
    public CancellationToken Token => _source.Token;

    public async Task OnQueryExecutedAsync(ExecutionResult result, CancellationToken cancellationToken)
    {
      if (result.Exception is null)
        return;

      Interlocked.Increment(ref _errorCount);
      if (_errorCount >= _errorLimit)
        _source.Cancel();
    }
  }
}
