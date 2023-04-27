using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Language;
using QueryPressure.WinUI.Services.Settings;

namespace QueryPressure.WinUI.Commands;

public class SetLanguageCommand : CommandBase<string>
{
  private readonly ILanguageService _languageService;
  private readonly ISettingsService _settingsService;
  private readonly HashSet<string> _availableLanguageSet;
  public SetLanguageCommand(ILogger<SetLanguageCommand> logger, ILanguageService languageService, ISettingsService settingsService) : base(logger)
  {
    _languageService = languageService;
    _settingsService = settingsService;
    _availableLanguageSet = _languageService.GetAvailableLanguages().ToHashSet(StringComparer.Ordinal);
  }

  protected override bool CanExecuteInternal(string? parameter)
  {
    return !string.IsNullOrEmpty(parameter) && _availableLanguageSet.Contains(parameter);
  }

  protected override void ExecuteInternal(string parameter)
  {
    _languageService.SetLanguage(parameter);
    _settingsService.SetLanguageSetting(parameter);
  }
}
