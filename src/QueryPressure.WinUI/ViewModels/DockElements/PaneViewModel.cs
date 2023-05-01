using QueryPressure.WinUI.Common;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QueryPressure.WinUI.ViewModels.DockElements;

public class PaneViewModel : ViewModelBase
{
  #region fields
  private string _title;
  private string _contentId;
  private bool _isSelected;
  private bool _isActive;
  #endregion fields

  #region constructors
  public PaneViewModel()
  {
    _title = string.Empty;
    _contentId = string.Empty;
    _isSelected = false;
    _isActive = false;
    IconSource = new BitmapImage();
  }
  #endregion constructors

  #region Properties
  public string Title
  {
    get => _title;
    set => SetField(ref _title, value);
  }

  public ImageSource IconSource { get; protected set; }

  public string ContentId
  {
    get => _contentId;
    set => SetField(ref _contentId, value);
  }

  public bool IsSelected
  {
    get => _isSelected;
    set => SetField(ref _isSelected, value);
  }

  public bool IsActive
  {
    get => _isActive;
    set=> SetField(ref _isActive, value);
  }
  #endregion Properties
}
