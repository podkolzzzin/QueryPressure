using System.Windows.Controls;
using System.Windows;
using QueryPressure.WinUI.ViewModels.ProjectTree;
using QueryPressure.WinUI.ViewModels.Properties;
using QueryPressure.WinUI.ViewModels.Execution;

namespace QueryPressure.WinUI.ViewModels.DockElements;

public class PanesTemplateSelector : DataTemplateSelector
{
  public DataTemplate? ProjectTreeViewTemplate { get; set; }
  public DataTemplate? PropertiesViewTemplate { get; set; }
  public DataTemplate? ScriptViewTemplate { get; set; }
  public DataTemplate? ExecutionResultsViewTemplate { get; set; }

  public override DataTemplate? SelectTemplate(object item, DependencyObject container)
  {
    return item switch
    {
      ProjectTreeViewModel => ProjectTreeViewTemplate,
      PropertiesViewModel => PropertiesViewTemplate,
      ScriptViewModel => ScriptViewTemplate,
      ExecutionViewModel => ExecutionResultsViewTemplate,
      _ => base.SelectTemplate(item, container)
    };
  }
}