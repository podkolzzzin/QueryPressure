using System.Globalization;
using QueryPressure.App.Interfaces;
using QueryPressure.WinUI.Common.Observer;

namespace QueryPressure.WinUI.Services.Language;

public class LanguageService : ILanguageService
{
  private readonly IResourceManager _resourceManager;
  private readonly ISubject<LanguageItem> _subject;

  private IDictionary<string, string>? _cache;
  private string _currentLocale;

  public LanguageService(IResourceManager resourceManager, ISubject<LanguageItem> subject)
  {
    _currentLocale = CultureInfo.CurrentCulture.Name;
    _resourceManager = resourceManager;
    _subject = subject;
  }

  public string GetCurrentLanguage() => _currentLocale;

  public IDictionary<string, string> GetStrings()
  {
    if (_cache is null)
    {
      throw new InvalidOperationException("Failed to get language strings. Need to set language before");
    }

    return _cache;
  }

  public void SetLanguage(string locale)
  {
    _currentLocale = locale;
    _cache = _resourceManager.GetResources(locale, ResourceFormat.Plain);
    _subject.Notify(this, new LanguageItem(locale, _cache));
  }

  public IReadOnlyList<string> GetAvailableLanguages() => new[] { "en-US", "uk-UA" };
}
