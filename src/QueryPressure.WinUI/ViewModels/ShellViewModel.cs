using QueryPressure.WinUI.Common;

namespace QueryPressure.WinUI.ViewModels;
public class ShellViewModel: ViewModelBase
{
  private string _title;
  private double _width;
  private double _height;

  public ShellViewModel()
  {
    _title = nameof(QueryPressure);
    _width = 800;
    _height = 600;
  }

  public string Title
  {
    get => _title;
    set => SetField(ref _title, value);
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
}
