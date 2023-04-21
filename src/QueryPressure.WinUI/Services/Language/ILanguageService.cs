namespace QueryPressure.WinUI.Services.Language;

public interface ILanguageService
{
  string GetCurrentLanguage();

  IDictionary<string, string> GetStrings();

  public void SetLanguage(string locale);

  IReadOnlyList<string> GetAvailableLanguages();
}
