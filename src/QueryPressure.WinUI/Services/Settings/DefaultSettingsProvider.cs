using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Services.Theme;

namespace QueryPressure.WinUI.Services.Settings;

public class DefaultSettingsProvider : IDefaultSettingsProvider
{
  private const ApplicationTheme DefaultTheme = ApplicationTheme.Light;

  private readonly ILogger<DefaultSettingsProvider> _logger;

  public DefaultSettingsProvider(ILogger<DefaultSettingsProvider> logger)
  {
    _logger = logger;
  }

  public Settings Get() => new()
  {
    Language = CultureInfo.CurrentUICulture.Name,
    Theme = GetDefaultTheme()
  };

  private ApplicationTheme GetDefaultTheme()
  {
    try
    {
      return ShouldSystemUseDarkMode() ? ApplicationTheme.Dark : ApplicationTheme.Light;
    }
    catch (Exception exception)
    {
      _logger.LogError(exception, "Failed to resolve default OS Theme. {DefaultTheme} will be used as a default",
        DefaultTheme);
      return DefaultTheme;
    }
  }

  [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
  public static extern bool ShouldSystemUseDarkMode();
}
