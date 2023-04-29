using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Services.Theme;

namespace QueryPressure.WinUI.ViewModels
{
  public class ThemeViewModel : ViewModelBase
  {
    private readonly ISubscription? _subscription;
    private ApplicationTheme _currentTheme;

    public ThemeViewModel(IObservableItem<ApplicationTheme>? themeObserver)
    {
      if (themeObserver != null)
      {
        _subscription = themeObserver.Subscribe(OnValueChanged);
        CurrentTheme = themeObserver.CurrentValue;
      }
      else
      {
        _subscription = null;
        CurrentTheme = ApplicationTheme.Light;
      }
    }

    private void OnValueChanged(ApplicationTheme value)
    {
      CurrentTheme = value;
    }

    public ApplicationTheme CurrentTheme
    {
      get => _currentTheme;
      set => SetField(ref _currentTheme, value);
    }

    public void Dispose()
    {
      _subscription?.Dispose();
    }
  }
}
