using QueryPressure.WinUI.ViewModels.DockElements;
using QueryPressure.WinUI.ViewModels.ProjectTree;
using QueryPressure.WinUI.ViewModels.Properties;
using System.Collections.ObjectModel;
using QueryPressure.WinUI.Common;

namespace QueryPressure.WinUI.ViewModels;

public class DockToolsViewModel : ViewModelBase
{
  private ScriptViewModel? _activeDocument;

  public DockToolsViewModel(ProjectTreeViewModel projectTree, PropertiesViewModel properties)
  {
    ProjectTree = projectTree;
    Properties = properties;
    Tools = new ToolViewModel[] { ProjectTree, Properties };
    Files = new ObservableCollection<ScriptViewModel>();
  }

  public ProjectTreeViewModel ProjectTree { get; }

  public PropertiesViewModel Properties { get; }

  public IReadOnlyList<ToolViewModel> Tools { get; }
  public ObservableCollection<ScriptViewModel> Files { get; }

  public ScriptViewModel? ActiveDocument
  {
    get => _activeDocument;
    set => SetField(ref _activeDocument, value);
  }
}
