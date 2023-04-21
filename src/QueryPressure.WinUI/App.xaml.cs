using System.Globalization;
using System.Windows;
using Autofac;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Views;

namespace QueryPressure.WinUI;

public partial class App : Application
{
  private readonly List<IDisposable> _subjects;
  private readonly IHost _host;

  public App()
  {
    _subjects = new List<IDisposable>();
    _host = Host.CreateDefaultBuilder()
      .UseServiceProviderFactory(new AutofacServiceProviderFactory())
      .ConfigureContainer<ContainerBuilder>(diBuilder => new WinApplicationLoader(_subjects).Load(diBuilder))
      .Build();
  }

  protected override async void OnStartup(StartupEventArgs e)
  {
    await _host.StartAsync();

    var shell = _host.Services.GetRequiredService<Shell>();

    var languageService = _host.Services.GetRequiredService<ILanguageService>();
    languageService.SetLanguage(CultureInfo.CurrentUICulture.Name);

    shell.Show();

    base.OnStartup(e);
  }

  protected override async void OnExit(ExitEventArgs e)
  {
    using (_host)
    {
      await _host.StopAsync(TimeSpan.FromSeconds(5));
    }

    foreach (var subject in _subjects)
    {
      subject.Dispose();
    }

    base.OnExit(e);
  }
}
