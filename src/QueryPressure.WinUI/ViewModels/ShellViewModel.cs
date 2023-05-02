using System.Windows;
using AvalonDock;
using AvalonDock.Layout.Serialization;
using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Services.Settings;
using QueryPressure.WinUI.Services.Theme;
using QueryPressure.WinUI.Services.WindowPosition;
using QueryPressure.WinUI.ViewModels.DockElements;
using QueryPressure.WinUI.ViewModels.ProjectTree;
using QueryPressure.WinUI.ViewModels.Properties;
using QueryPressure.WinUI.Views;

namespace QueryPressure.WinUI.ViewModels;

public class ShellViewModel : ViewModelBase
{
  private readonly IWindowPositionService _positionService;
  private readonly ISettingsService _settingsService;
  private readonly IThemeService _themeService;

  public ShellViewModel(IWindowPositionService positionService, ISettingsService settingsService,
    IThemeService themeService,
    MenuViewModel menu, ProjectTreeViewModel projectTree, PropertiesViewModel properties)
  {
    _positionService = positionService;
    _settingsService = settingsService;
    _themeService = themeService;
    Menu = menu;
    ProjectTree = projectTree;
    Properties = properties;
    Tools = new ToolViewModel[] { ProjectTree, Properties };
  }

  private void SetWindowPosition(Window shell)
  {
    var windowSettings = _settingsService.GetWindowSettings();
    _positionService.SetSettings(shell, windowSettings);
  }

  private void SaveWindowPosition(Window shell)
  {
    var windowSettings = _positionService.GetSettings(shell);
    _settingsService.SetWindowSettings(windowSettings);
  }

  private void LoadDockLayout(DockingManager dockManager)
  {
    _settingsService.LoadDockLayout(GetSerializer(dockManager));
  }

  private static XmlLayoutSerializer GetSerializer(DockingManager dockManager)
  {
    var serializer = new XmlLayoutSerializer(dockManager);
    serializer.LayoutSerializationCallback += (s, args) =>
    {
      args.Content = args.Content;
    };

    return serializer;
  }

  private void SaveDockLayout(DockingManager dockManager)
  {
    _settingsService.SetDockLayout(GetSerializer(dockManager));
  }

  public void OnWindowSourceInitialized(Shell shell)
  {
    SetWindowTheme(shell);
    SetWindowPosition(shell);
    LoadDockLayout(shell.DockManager);
  }

  private void SetWindowTheme(Shell shell)
  {
    _themeService.SetWindowTheme(shell, _settingsService.GetApplicationTheme());
  }

  public void OnWindowsClosing(Shell shell)
  {
    SaveWindowPosition(shell);
    SaveDockLayout(shell.DockManager);
  }

  public MenuViewModel Menu { get; }

  public ProjectTreeViewModel ProjectTree { get; }

  public PropertiesViewModel Properties { get; }

  public IEnumerable<ToolViewModel> Tools { get; }
}
