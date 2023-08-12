using System.Windows;
using QueryPressure.WinUI.Services.Settings;

namespace QueryPressure.WinUI.Services.WindowPosition;

public interface IWindowPositionService
{
  void SetSettings(Window window, WindowSettings settings);
  WindowSettings GetSettings(Window window);
}
