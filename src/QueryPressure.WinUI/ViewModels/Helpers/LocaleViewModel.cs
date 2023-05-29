using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Services.Language;

namespace QueryPressure.WinUI.ViewModels.Helpers;

public class LocaleViewModel : ViewModelBase, IDisposable
{
  private readonly ISubscription? _languageSubscription;
  private string _currentLanguage;
  private IDictionary<string, string> _strings;

  public LocaleViewModel(IObservableItem<LanguageItem>? languageObserver)
  {
    if (languageObserver != null)
    {
      _languageSubscription = languageObserver.Subscribe(OnLanguageValueChanged);
      _currentLanguage = languageObserver.CurrentValue.Locale;
      _strings = languageObserver.CurrentValue.Strings;
    }
    else
    {
      _languageSubscription = null;
      _currentLanguage = "";
      _strings = new Dictionary<string, string>();
    }
  }

  private void OnLanguageValueChanged(object? sender, LanguageItem value)
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
    _languageSubscription?.Dispose();
  }
}
