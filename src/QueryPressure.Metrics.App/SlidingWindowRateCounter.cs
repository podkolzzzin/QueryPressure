using System.Collections.Concurrent;

namespace QueryPressure.Metrics.App;

public class SlidingWindowRateCounter
{
  private readonly ConcurrentQueue<DateTime> _callTimes = new ();
  private readonly TimeSpan _windowSize;

  public SlidingWindowRateCounter(TimeSpan windowSize)
  {
    _windowSize = windowSize;
  }

  public void RegisterCall()
  {
    CleanUpQueue();
    _callTimes.Enqueue(DateTime.UtcNow);
  }
  
  public int GetCallsPerTimeWindow() => CleanUpQueue();

  private int CleanUpQueue()
  {
    while (_callTimes.TryPeek(out var oldestCall) && DateTime.UtcNow - oldestCall > _windowSize)
    {
      _callTimes.TryDequeue(out _);
    }
    return _callTimes.Count;
  }
}
