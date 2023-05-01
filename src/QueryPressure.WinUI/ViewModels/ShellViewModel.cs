using System.Windows;
using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Services.Settings;
using QueryPressure.WinUI.Services.WindowPosition;
using QueryPressure.WinUI.ViewModels.DockElements;
using QueryPressure.WinUI.ViewModels.ProjectTree;

namespace QueryPressure.WinUI.ViewModels;

public class ShellViewModel : ViewModelBase
{
  private readonly IWindowPositionService _positionService;
  private readonly ISettingsService _settingsService;

  public ShellViewModel(IWindowPositionService positionService, ISettingsService settingsService,
    MenuViewModel menu, ProjectTreeViewModel projectTree)
  {
    _positionService = positionService;
    _settingsService = settingsService;
    Menu = menu;
    ProjectTree = projectTree;
    Tools = new[] {ProjectTree};
  }

  public void SetWindowPosition(Window shell)
  {
    var windowSettings = _settingsService.GetWindowSettings();
    _positionService.SetSettings(shell, windowSettings);
  }

  public void SaveWindowPosition(Window shell)
  {
    var windowSettings = _positionService.GetSettings(shell);
    _settingsService.SetWindowSettings(windowSettings);
  }

  public MenuViewModel Menu { get; }

  public ProjectTreeViewModel ProjectTree { get; }

  public IEnumerable<ToolViewModel> Tools { get; }
}
