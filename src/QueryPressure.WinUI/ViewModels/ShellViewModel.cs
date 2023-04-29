using System.Windows;
using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Settings;
using QueryPressure.WinUI.Services.Theme;
using QueryPressure.WinUI.Services.WindowPosition;

namespace QueryPressure.WinUI.ViewModels;

public class ShellViewModel : ViewModelBase
{
  private readonly IWindowPositionService _positionService;
  private readonly ISettingsService _settingsService;

  public ShellViewModel(IWindowPositionService positionService, ISettingsService settingsService,
    ILanguageService languageService, IThemeService themeService)
  {
    _positionService = positionService;
    _settingsService = settingsService;
    Languages = languageService.GetAvailableLanguages();
    Themes = themeService.GetAvailableThemes();
  }

  public void SetWindowPosition(Window shell)
  {
    var windowSettings = _settingsService.GetWindowSettings();
    _positionService.SetSettings(shell, windowSettings);
  }

  public void SaveWindowPosition(Window shell)
  {
    var windowSettings = _positionService.GetSettings(shell);
    _settingsService.SetWindowSettings(windowSettings);
  }

  public IReadOnlyList<ApplicationTheme> Themes { get; set; }
  public IReadOnlyList<string> Languages { get; }
}
