using QueryPressure.WinUI.Common;

namespace QueryPressure.WinUI.ViewModels;

public abstract class LocalizedViewModel : ViewModelBase
{
  protected LocalizedViewModel(LocaleViewModel locale)
  {
    Locale = locale;
  }

  public LocaleViewModel Locale { get; }
}
