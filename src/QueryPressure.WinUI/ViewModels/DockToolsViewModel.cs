using QueryPressure.WinUI.ViewModels.DockElements;
using QueryPressure.WinUI.ViewModels.ProjectTree;
using QueryPressure.WinUI.ViewModels.Properties;

namespace QueryPressure.WinUI.ViewModels;

public class DockToolsViewModel
{
  public DockToolsViewModel(ProjectTreeViewModel projectTree, PropertiesViewModel properties)
  {
    ProjectTree = projectTree;
    Properties = properties;
    Tools = new ToolViewModel[] { ProjectTree, Properties };
  }

  public ProjectTreeViewModel ProjectTree { get; }

  public PropertiesViewModel Properties { get; }

  public IReadOnlyList<ToolViewModel> Tools { get; }
}
