using System.Windows.Controls;
using System.Windows;
using QueryPressure.WinUI.ViewModels.ProjectTree;

namespace QueryPressure.WinUI.ViewModels.DockElements;

class PanesTemplateSelector : DataTemplateSelector
{
  public PanesTemplateSelector()
  {
  }

  public DataTemplate? ProjectTreeViewTemplate { get; set; }

  public override DataTemplate? SelectTemplate(object item, DependencyObject container)
  {
    if (item is ProjectTreeViewModel)
      return ProjectTreeViewTemplate;

    return base.SelectTemplate(item, container);
  }
}
