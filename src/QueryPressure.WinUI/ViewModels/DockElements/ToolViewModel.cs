namespace QueryPressure.WinUI.ViewModels.DockElements;

public class ToolViewModel : PaneViewModel
{
  #region fields
  private bool _isVisible = true;
  #endregion fields

  #region constructor
  /// <summary>
  /// Class constructor
  /// </summary>
  /// <param name="name"></param>
  public ToolViewModel(string name)
  {
    Title = name;
    ContentId = name;
  }
  #endregion constructor

  #region Properties

  public bool IsVisible
  {
    get => _isVisible;
    set => SetField(ref _isVisible, value);
  }
  #endregion Properties
}
