namespace QueryPressure.WinUI.Configuration;

public class UserSettingsOptions
{
  public UserSettingsOptions()
  {
    Path = "settings.json";
    LayoutPath = "layout.config";
  }

  public string Path { get; set; }
  public string LayoutPath { get; set; }
}
