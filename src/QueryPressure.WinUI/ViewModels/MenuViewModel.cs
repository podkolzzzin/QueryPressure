using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Theme;

namespace QueryPressure.WinUI.ViewModels;

public class MenuViewModel : ViewModelBase
{
  public MenuViewModel(ILanguageService languageService, IThemeService themeService)
  {
    Languages = languageService.GetAvailableLanguages();
    Themes = themeService.GetAvailableThemes();
  }
  public IReadOnlyList<ApplicationTheme> Themes { get; set; }
  public IReadOnlyList<string> Languages { get; }
}
