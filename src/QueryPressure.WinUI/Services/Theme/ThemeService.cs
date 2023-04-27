using System.Windows;

namespace QueryPressure.WinUI.Services.Theme;

public class ThemeService : IThemeService
{
  private readonly App _application;

  public ThemeService(App application)
  {
    _application = application;
  }

  public void Set(ApplicationTheme theme)
  {
    var themeUri = GetThemeUri(theme);
    _application.Resources.MergedDictionaries.Clear();
    _application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(@"Themes/Common/CommonResources.xaml", UriKind.Relative) });
    _application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(themeUri, UriKind.Relative) });
  }

  private static string GetThemeUri(ApplicationTheme theme)
  {
    return theme switch
    {
      ApplicationTheme.Dark => @"Themes/Dark/DarkTheme.xaml",
      ApplicationTheme.Light => @"Themes/Light/LightTheme.xaml",
      _ => throw new ArgumentOutOfRangeException(nameof(theme), theme, null)
    };
  }
}
