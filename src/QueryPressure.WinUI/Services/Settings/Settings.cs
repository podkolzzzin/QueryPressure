using QueryPressure.WinUI.Services.Theme;

namespace QueryPressure.WinUI.Services.Settings;

public struct Settings
{
  public Settings()
  {
    Language = "en-US";
    WindowSettings = new WindowSettings();
    Theme = ApplicationTheme.Light;
  }

  public WindowSettings WindowSettings { get; set; }
  public string Language { get; set; }
  public ApplicationTheme Theme { get; set; }
}
