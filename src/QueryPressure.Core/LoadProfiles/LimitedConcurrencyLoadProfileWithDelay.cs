using QueryPressure.Core.Interfaces;

namespace QueryPressure.Core.LoadProfiles;

public class LimitedConcurrencyLoadProfileWithDelay : IProfile
{
  private readonly TimeSpan _delay;
  private readonly IProfile _internal;

  public LimitedConcurrencyLoadProfileWithDelay(int limit, TimeSpan delay)
  {
    _delay = delay;
    _internal = new LimitedConcurrencyLoadProfile(limit);
  }

  public Task<bool> WhenNextCanBeExecutedAsync(CancellationToken cancellationToken)
  {
    return _internal.WhenNextCanBeExecutedAsync(cancellationToken);
  }

  public async Task OnQueryExecutedAsync(CancellationToken cancellationToken)
  {
    await Task.Delay(_delay, cancellationToken);
    await _internal.OnQueryExecutedAsync(cancellationToken);
  }
}
