using System.IO;
using System.Reflection;
using System.Text.Json;
using AvalonDock.Layout.Serialization;
using Microsoft.Extensions.Options;
using QueryPressure.WinUI.Configuration;
using QueryPressure.WinUI.Services.Theme;

namespace QueryPressure.WinUI.Services.Settings;

public interface ISettingsService
{
  Task LoadAsync(CancellationToken token);
  Task SaveAsync(CancellationToken token);

  WindowSettings GetWindowSettings();
  void SetWindowSettings(WindowSettings windowSettings);

  string GetLanguageSetting();
  void SetLanguageSetting(string languageSetting);

  ApplicationTheme GetApplicationTheme();
  void SetApplicationTheme(ApplicationTheme applicationTheme);

  void LoadDockLayout(XmlLayoutSerializer serializer);
  void SetDockLayout(XmlLayoutSerializer serializer);
}

public class SettingsService : ISettingsService
{
  private readonly IOptionsMonitor<UserSettingsOptions> _userSettingsOptions;

  private Settings _settingsCache;

  public SettingsService(IOptionsMonitor<UserSettingsOptions> userSettingsOptions, IDefaultSettingsProvider defaultSettingsProvider)
  {
    _userSettingsOptions = userSettingsOptions;
    _settingsCache = defaultSettingsProvider.Get();
  }

  public async Task LoadAsync(CancellationToken token)
  {
    var settingsFile = new FileInfo(Environment.ExpandEnvironmentVariables(_userSettingsOptions.CurrentValue.Path));
    if (settingsFile.Exists)
    {
      await using var stream = settingsFile.OpenRead();
      _settingsCache = await JsonSerializer.DeserializeAsync<Settings>(stream, cancellationToken: token);
    }
  }

  public async Task SaveAsync(CancellationToken token)
  {
    await using var stream = File.Create(Environment.ExpandEnvironmentVariables(_userSettingsOptions.CurrentValue.Path));
    await JsonSerializer.SerializeAsync(stream, _settingsCache, cancellationToken: token);
  }

  public WindowSettings GetWindowSettings() => _settingsCache.WindowSettings;

  public void SetWindowSettings(WindowSettings windowSettings) => _settingsCache.WindowSettings = windowSettings;

  public string GetLanguageSetting() => _settingsCache.Language;

  public void SetLanguageSetting(string languageSetting) => _settingsCache.Language = languageSetting;

  public ApplicationTheme GetApplicationTheme() => _settingsCache.Theme;

  public void SetApplicationTheme(ApplicationTheme applicationTheme) => _settingsCache.Theme = applicationTheme;

  public void LoadDockLayout(XmlLayoutSerializer serializer)
  {
    var settingsFile = new FileInfo(Environment.ExpandEnvironmentVariables(_userSettingsOptions.CurrentValue.LayoutPath));
    if (settingsFile.Exists)
    {
      serializer.Deserialize(settingsFile.FullName);
    }
    else
    {
      LoadDefaultDockLayout(serializer);
    }
  }

  private static void LoadDefaultDockLayout(XmlLayoutSerializer serializer)
  {
    var assembly = Assembly.GetExecutingAssembly();
    const string defaultDockLayoutConfig = "QueryPressure.WinUI.Resources.default.dock.layout.config";

    using var stream = assembly.GetManifestResourceStream(defaultDockLayoutConfig);
    if (stream == null)
    {
      throw new InvalidOperationException($"Cannot find the Default Dock Layout resource file ('{defaultDockLayoutConfig}') in the current assemble");
    }

    using var reader = new StreamReader(stream);
    serializer.Deserialize(reader);
  }

  public void SetDockLayout(XmlLayoutSerializer serializer)
  {
    var filePath = Environment.ExpandEnvironmentVariables(_userSettingsOptions.CurrentValue.LayoutPath);
    serializer.Serialize(filePath);
  }
}
