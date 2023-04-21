using System.Globalization;
using System.Windows.Input;
using QueryPressure.WinUI.Commands;
using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Common.Observer;
using QueryPressure.WinUI.Services.Language;

namespace QueryPressure.WinUI.ViewModels;
public class ShellViewModel : ViewModelBase, IDisposable
{
  private string _title;
  private string _currentLanguage;
  private double _width;
  private double _height;
  private readonly ISubscription _languageSubscription;

  public ShellViewModel(IObservableItem<LanguageItem> languageObserver, SetLanguageCommand setLanguageCommand)
  {
    _title = nameof(QueryPressure);
    _currentLanguage = CultureInfo.CurrentUICulture.Name;
    _width = 800;
    _height = 600;
    _languageSubscription = languageObserver.Subscribe(OnLanguageValueChanged);
    SetLanguageCommand = setLanguageCommand;
  }

  private void OnLanguageValueChanged(LanguageItem value)
  {
    Title = value.Strings["labels.title"];
    CurrentLanguage = value.Locale;
  }

  public ICommand SetLanguageCommand { get; }

  public string Title
  {
    get => _title;
    set => SetField(ref _title, value);
  }

  public string CurrentLanguage
  {
    get => _currentLanguage;
    set => SetField(ref _currentLanguage, value);
  }

  public double Width
  {
    get => _width;
    set => SetField(ref _width, value);
  }

  public double Height
  {
    get => _height;
    set => SetField(ref _height, value);
  }

  public void Dispose()
  {
    _languageSubscription.Dispose();
  }
}
