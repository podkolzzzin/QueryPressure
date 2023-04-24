using System.Windows;
using System.Windows.Input;
using QueryPressure.WinUI.Commands;
using QueryPressure.WinUI.Services.Settings;
using QueryPressure.WinUI.Services.WindowPosition;

namespace QueryPressure.WinUI.ViewModels;

public class ShellViewModel : LocalizedViewModel
{
  private readonly IWindowPositionService _positionService;
  private readonly ISettingsService _settingsService;

  public ShellViewModel(LocaleViewModel locale, IWindowPositionService positionService, ISettingsService settingsService,
    SetLanguageCommand setLanguageCommand) : base(locale)
  {
    _positionService = positionService;
    _settingsService = settingsService;
    SetLanguageCommand = setLanguageCommand;
  }

  public ICommand SetLanguageCommand { get; }

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
