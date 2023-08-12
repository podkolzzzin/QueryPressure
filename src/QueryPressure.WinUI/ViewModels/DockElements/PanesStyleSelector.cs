using System.Windows.Controls;
using System.Windows;

namespace QueryPressure.WinUI.ViewModels.DockElements;

public class PanesStyleSelector : StyleSelector
{
  public Style? ToolStyle { get; set; }

  public Style? FileStyle { get; set; }

  public override Style? SelectStyle(object item, DependencyObject container)
  {
    return item switch
    {
      ToolViewModel => ToolStyle,
      PaneViewModel => FileStyle,
      _ => base.SelectStyle(item, container)
    };
  }
}
