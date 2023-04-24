namespace QueryPressure.WinUI.Configuration;

public class UserSettingsOptions
{
  public UserSettingsOptions()
  {
    Path = "settings.json";
  }

  public string Path { get; set; }
}
