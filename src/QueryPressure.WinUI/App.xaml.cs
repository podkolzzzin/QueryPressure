using System.Windows;
using Autofac;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using QueryPressure.WinUI.Views;

namespace QueryPressure.WinUI;

public partial class App : Application
{
  private readonly IHost _host;

  public App()
  {
    _host = Host.CreateDefaultBuilder()
      .UseServiceProviderFactory(new AutofacServiceProviderFactory())
      .ConfigureContainer<ContainerBuilder>(diBuilder => new WinApplicationLoader().Load(diBuilder))
      .Build();
  }

  protected override async void OnStartup(StartupEventArgs e)
  {
    await _host.StartAsync();

    var shell = _host.Services.GetRequiredService<Shell>();
    shell.Show();

    base.OnStartup(e);
  }

  protected override async void OnExit(ExitEventArgs e)
  {
    using (_host)
    {
      await _host.StopAsync(TimeSpan.FromSeconds(5));
    }

    base.OnExit(e);
  }
}
