using System.Windows.Controls;
using System.Windows;

namespace QueryPressure.WinUI.ViewModels.DockElements;

class PanesStyleSelector : StyleSelector
{
  public Style? ToolStyle { get; set; }

  public Style? FileStyle { get; set; }

  public override Style? SelectStyle(object item, DependencyObject container)
  {
    if (item is ToolViewModel)
      return ToolStyle;

    return base.SelectStyle(item, container);
  }
}
