using System.Diagnostics;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace QueryPressure.UI;

public class Launcher
{
  private static readonly TimeSpan WaitForServerTimout = TimeSpan.FromMinutes(1);
  private readonly IServer _server;
  public Launcher(IServer server)
  {
    _server = server;
  }

  public async Task Start(CancellationToken cancellationToken)
  {
    var host = await WhenServerIsReadyAsync(cancellationToken);
    var url = new Uri(host, "UI");
    Process.Start(new ProcessStartInfo(url.ToString())
    {
      UseShellExecute = true,
    });
  }

  private async Task<Uri> WhenServerIsReadyAsync(CancellationToken cancellationToken)
  {
    var sw = Stopwatch.StartNew();
    IServerAddressesFeature? addresses;
    do
    {
      addresses = _server.Features.Get<IServerAddressesFeature>();
      if (addresses!.Addresses.Count > 0)
        break;
      await Task.Delay(100, cancellationToken);
    } while (sw.Elapsed < WaitForServerTimout);

    return new Uri(addresses.Addresses.Select(x => x.Replace("[::]", "localhost").Replace("+:", "localhost:"))
      .Single());
  }
}
