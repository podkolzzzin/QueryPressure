namespace QueryPressure.UI.HostedServices;

public abstract class BackgroundInfiniteService : BackgroundService
{
  protected abstract Task ExecuteRepeatedJobAsync(CancellationToken stoppingToken);
  
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      await ExecuteRepeatedJobAsync(stoppingToken);
    }
  }
}
