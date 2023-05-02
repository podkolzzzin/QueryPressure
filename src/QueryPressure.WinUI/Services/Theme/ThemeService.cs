using QueryPressure.WinUI.Common.Observer;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace QueryPressure.WinUI.Services.Theme;

public class ThemeService : IThemeService
{
  #region Win32 API declarations to set and get window attribute

  private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

  [DllImport("dwmapi.dll", PreserveSig = true)]
  public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref bool attrValue, int attrSize);

  #endregion

  private readonly App _application;
  private readonly ISubject<ApplicationTheme> _subject;

  public ThemeService(App application, ISubject<ApplicationTheme> subject)
  {
    _application = application;
    _subject = subject;
  }

  public void Set(ApplicationTheme theme)
  {
    var themeUri = GetThemeUri(theme);
    SetWindowTheme(_application.MainWindow, theme);
    _application.Resources.MergedDictionaries.Clear();
    _application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(@"Themes/Common/CommonResources.xaml", UriKind.Relative) });
    _application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(themeUri, UriKind.Relative) });
    _subject.Notify(theme);
  }

  public void SetWindowTheme(Window? window, ApplicationTheme theme)
  {
    if (window == null)
    {
      return;
    }

    var value = theme == ApplicationTheme.Dark;

    try
    {
      DwmSetWindowAttribute(new WindowInteropHelper(window).Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, Marshal.SizeOf(value));
    }
    catch
    {
      // ignored TODO Logging
    }

  }

  public IReadOnlyList<ApplicationTheme> GetAvailableThemes()
  {
    return Enum.GetValues<ApplicationTheme>();
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
