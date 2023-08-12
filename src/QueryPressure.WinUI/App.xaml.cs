using System.Windows;
using Autofac;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using QueryPressure.WinUI.Configuration;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Views;
using QueryPressure.WinUI.Services.Settings;
using QueryPressure.WinUI.Services.Theme;

namespace QueryPressure.WinUI;

public partial class App : Application
{
  private readonly List<IDisposable> _subjects;
  private readonly IHost _host;

  public App()
  {
    _subjects = new List<IDisposable>();
    _host = Host.CreateDefaultBuilder()

#if DEBUG
      .UseEnvironment("Development")
#endif

      .UseServiceProviderFactory(new AutofacServiceProviderFactory())
      .ConfigureContainer<ContainerBuilder>(diBuilder => new WinApplicationLoader(this, _subjects).Load(diBuilder))
      .ConfigureServices(ConfigureServices)
      .Build();
  }

  public IServiceProvider ServiceProvider => _host.Services;

  private static void ConfigureServices(HostBuilderContext builder, IServiceCollection services)
  {
    services.Configure<UserSettingsOptions>(builder.Configuration.GetSection("UserSettings"));
  }

  protected override async void OnStartup(StartupEventArgs e)
  {
    var token = CancellationToken.None;

    await _host.StartAsync(token);

    await LoadSettingsAsync(token);

    var shell = _host.Services.GetRequiredService<Shell>();
    shell.Show();

    base.OnStartup(e);
  }

  private async Task LoadSettingsAsync(CancellationToken token)
  {
    var settingsService = _host.Services.GetRequiredService<ISettingsService>();
    await settingsService.LoadAsync(token);

    var languageService = _host.Services.GetRequiredService<ILanguageService>();
    languageService.SetLanguage(settingsService.GetLanguageSetting());

    var themeService = _host.Services.GetRequiredService<IThemeService>();
    themeService.Set(settingsService.GetApplicationTheme());
  }

  protected override async void OnExit(ExitEventArgs e)
  {
    using (_host)
    {
      var token = CancellationToken.None;
      var settingsService = _host.Services.GetRequiredService<ISettingsService>();

      await settingsService.SaveAsync(token);
      await _host.StopAsync(TimeSpan.FromSeconds(5));
    }

    foreach (var subject in _subjects)
    {
      subject.Dispose();
    }

    base.OnExit(e);
  }
}
