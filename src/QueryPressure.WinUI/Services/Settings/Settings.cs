using System.Globalization;

namespace QueryPressure.WinUI.Services.Settings;

public struct Settings
{
  public Settings()
  {
    Language = CultureInfo.CurrentUICulture.Name;
    WindowSettings = new WindowSettings();
  }

  public WindowSettings WindowSettings { get; set; }
  public string Language { get; set; }
}
