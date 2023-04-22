using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Services.Language;
using System.Globalization;

namespace QueryPressure.WinUI.ViewModels;

public class LocaleViewModel : ViewModelBase, IDisposable
{
  private readonly ISubscription _languageSubscription;
  private string _currentLanguage;
  private IDictionary<string, string> _strings;

  public LocaleViewModel(IObservableItem<LanguageItem> languageObserver)
  {
    _languageSubscription = languageObserver.Subscribe(OnLanguageValueChanged);
    _currentLanguage = CultureInfo.CurrentUICulture.Name;
    _strings = new Dictionary<string, string>();
  }

  private void OnLanguageValueChanged(LanguageItem value)
  {
    Strings = value.Strings;
    CurrentLanguage = value.Locale;
  }

  public IDictionary<string, string> Strings
  {
    get => _strings;
    set => SetField(ref _strings, value);
  }

  public string CurrentLanguage
  {
    get => _currentLanguage;
    set => SetField(ref _currentLanguage, value);
  }
  public void Dispose()
  {
    _languageSubscription.Dispose();
  }
}
