using System.Globalization;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Options;
using QueryPressure.WinUI.Configuration;

namespace QueryPressure.WinUI.Services.Settings;

public interface ISettingsService
{
  Task LoadAsync(CancellationToken token);
  Task SaveAsync(CancellationToken token);

  WindowSettings GetWindowSettings();
  void SetWindowSettings(WindowSettings windowSettings);

  string GetLanguageSetting();
  void SetLanguageSetting(string languageSetting);
}

public class SettingsService : ISettingsService
{
  private readonly IOptionsMonitor<UserSettingsOptions> _userSettingsOptions;

  private Settings _settingsCache;

  public SettingsService(IOptionsMonitor<UserSettingsOptions> userSettingsOptions)
  {
    _userSettingsOptions = userSettingsOptions;
    _settingsCache = new Settings();
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
}
