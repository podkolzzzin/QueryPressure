using System.Windows.Controls;
using System.Windows;
using QueryPressure.WinUI.ViewModels.ProjectTree;
using QueryPressure.WinUI.ViewModels.Properties;

namespace QueryPressure.WinUI.ViewModels.DockElements;

public class PanesTemplateSelector : DataTemplateSelector
{
  public DataTemplate? ProjectTreeViewTemplate { get; set; }
  public DataTemplate? PropertiesViewTemplate { get; set; }
  public DataTemplate? ScriptViewTemplate { get; set; }

  public override DataTemplate? SelectTemplate(object item, DependencyObject container)
  {
    return item switch
    {
      ProjectTreeViewModel => ProjectTreeViewTemplate,
      PropertiesViewModel => PropertiesViewTemplate,
      ScriptViewModel => ScriptViewTemplate,
      _ => base.SelectTemplate(item, container)
    };
  }
}
