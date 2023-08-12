using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Theme;
using QueryPressure.WinUI.ViewModels.DockElements;

namespace QueryPressure.WinUI.ViewModels;

public class MenuViewModel : ViewModelBase
{
  public MenuViewModel(ILanguageService languageService, IThemeService themeService, DockToolsViewModel dockTools)
  {
    Languages = languageService.GetAvailableLanguages();
    Themes = themeService.GetAvailableThemes();
    Tools = dockTools.Tools.ToList();
  }

  public IReadOnlyList<ToolViewModel> Tools { get; set; }
  public IReadOnlyList<ApplicationTheme> Themes { get; set; }
  public IReadOnlyList<string> Languages { get; }
}
