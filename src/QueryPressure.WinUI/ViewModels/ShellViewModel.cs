using System.Windows;
using QueryPressure.WinUI.Common;
using QueryPressure.WinUI.Services.Settings;
using QueryPressure.WinUI.Services.WindowPosition;

namespace QueryPressure.WinUI.ViewModels;

public class ShellViewModel : ViewModelBase
{
  private readonly IWindowPositionService _positionService;
  private readonly ISettingsService _settingsService;

  public ShellViewModel(IWindowPositionService positionService, ISettingsService settingsService)
  {
    _positionService = positionService;
    _settingsService = settingsService;
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
}
