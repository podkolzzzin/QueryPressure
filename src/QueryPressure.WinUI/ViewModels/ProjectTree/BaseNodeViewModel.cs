using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Models;

namespace QueryPressure.WinUI.ViewModels.ProjectTree;
public class BaseNodeViewModel : ViewModelBase
{
  protected IModel Model;

  public BaseNodeViewModel(BaseNodeViewModel? parent, IModel model)
  {
    Model = model;
  }
}
