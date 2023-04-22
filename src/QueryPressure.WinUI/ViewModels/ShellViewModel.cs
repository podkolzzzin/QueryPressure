using System.Windows.Input;
using QueryPressure.WinUI.Commands;

namespace QueryPressure.WinUI.ViewModels;

public class ShellViewModel : LocalizedViewModel
{
  private double _width;
  private double _height;

  public ShellViewModel(LocaleViewModel locale, SetLanguageCommand setLanguageCommand) : base(locale)
  {
    _width = 800;
    _height = 600;
    SetLanguageCommand = setLanguageCommand;
  }

  public ICommand SetLanguageCommand { get; }

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
}
