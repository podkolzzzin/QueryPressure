using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.Limits
{
  public sealed class TillNErrorsLimit : ILimit
  {
    private readonly CancellationTokenSource _source;
    private readonly int _errorLimit;
    private int _errorCount;

    public TillNErrorsLimit(int errorLimit)
    {
      _source = new();
      _errorLimit = errorLimit;
      _errorCount = 0;
    }
    public CancellationToken Token => _source.Token;
    public void OnErrorOccured()
    {
      Interlocked.Increment(ref _errorCount);

      if (_errorCount >= _errorLimit)
        _source.Cancel();
    }
  }
}
