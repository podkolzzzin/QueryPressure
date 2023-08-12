using System.Windows;

namespace QueryPressure.WinUI.Services.Theme;

public interface IThemeService
{
  void Set(ApplicationTheme locale);
  IReadOnlyList<ApplicationTheme> GetAvailableThemes();
  void SetWindowTheme(Window window, ApplicationTheme theme);
}
