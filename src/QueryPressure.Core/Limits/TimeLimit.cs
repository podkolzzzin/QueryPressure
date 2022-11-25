using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.Limits;

public class TimeLimit : ILimit
{
  private readonly CancellationTokenSource _source;

  public TimeLimit(TimeSpan limit)
  {
    _source = new(limit);
  }
  public CancellationToken Token => _source.Token;
}
