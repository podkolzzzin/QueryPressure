using QueryPressure.WinUI.Common.Commands;
using QueryPressure.WinUI.Services.Language;

namespace QueryPressure.WinUI.Commands;

public class SetLanguageCommand : CommandBase<string>
{
  private readonly ILanguageService _languageService;
  private readonly HashSet<string> _availableLanguageSet;
  public SetLanguageCommand(ILanguageService languageService)
  {
    _languageService = languageService;
    _availableLanguageSet = _languageService.GetAvailableLanguages().ToHashSet(StringComparer.Ordinal);
  }

  protected override bool CanExecuteInternal(string? parameter)
  {
    return !string.IsNullOrEmpty(parameter) && _availableLanguageSet.Contains(parameter);
  }

  protected override void ExecuteInternal(string parameter)
  {
    _languageService.SetLanguage(parameter);
  }
}
