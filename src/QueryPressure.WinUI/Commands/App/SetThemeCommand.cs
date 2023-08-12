using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Settings;
using QueryPressure.WinUI.Services.Theme;

namespace QueryPressure.WinUI.Commands.App;

public class SetThemeCommand : CommandBase<ApplicationTheme>
{
  private readonly IThemeService _themeService;
  private readonly ISettingsService _settingsService;

  public SetThemeCommand(ILogger<SetThemeCommand> logger, IThemeService themeService, ISettingsService settingsService) : base(logger)
  {
    _themeService = themeService;
    _settingsService = settingsService;
  }

  protected override void ExecuteInternal(ApplicationTheme theme)
  {
    _themeService.Set(theme);
    _settingsService.SetApplicationTheme(theme);
  }
}
