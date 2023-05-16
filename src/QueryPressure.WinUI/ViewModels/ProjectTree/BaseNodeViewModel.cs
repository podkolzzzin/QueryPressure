using System.Windows.Input;
using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;

public class BaseNodeViewModel : ViewModelBase
{
  private bool _isExpanded;
  private bool _isSelected;

  public IModel Model;

  public BaseNodeViewModel(IModel model, bool isSupportSubNodes = false)
  {
    Model = model;

    if (isSupportSubNodes)
    {
      Nodes = new List<BaseNodeViewModel>();
    }
  }

  public List<BaseNodeViewModel>? Nodes { get; }

  public bool IsExpanded
  {
    get => _isExpanded;
    set => SetField(ref _isExpanded, value);
  }

  public bool IsSelected
  {
    get => _isSelected;
    set => SetField(ref _isSelected, value);
  }

  public virtual void Click(MouseButtonEventArgs args, bool isDoubleClick = false)
  {
  }
}
