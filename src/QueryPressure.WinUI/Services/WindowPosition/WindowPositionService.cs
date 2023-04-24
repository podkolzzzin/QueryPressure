using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Extensions.Logging;
using QueryPressure.WinUI.Services.Settings;

namespace QueryPressure.WinUI.Services.WindowPosition;

public class WindowPositionService : IWindowPositionService
{
  private readonly ILogger<WindowPositionService> _logger;

  public WindowPositionService(ILogger<WindowPositionService> logger)
  {
    _logger = logger;
  }

  #region Win32 API declarations to set and get window placement

  [DllImport("user32.dll")]
  private static extern bool SetWindowPlacement(IntPtr windowHandle, [In] ref WindowPlacement settings);

  [DllImport("user32.dll")]
  private static extern bool GetWindowPlacement(IntPtr windowHandle, out WindowPlacement settings);

  private const int SwShowNormal = 1;
  private const int SwShowMinimized = 2;

  #endregion

  public void SetSettings(Window window, WindowSettings settings)
  {
    if (AreSettingsEmpty(settings))
    {
      // If setting empty - nothing to do (use WPF XAML Default)
      return;
    }

    try
    {
      // Load window placement details for previous application session from application settings
      // Note - if window was closed on a monitor that is now disconnected from the computer,
      //        SetWindowPlacement will place the window onto a visible monitor.
      var windowPlacement = GetPlacement(settings);
      windowPlacement.length = Marshal.SizeOf(typeof(WindowPlacement));
      windowPlacement.flags = 0;
      windowPlacement.showCmd = (windowPlacement.showCmd == SwShowMinimized ? SwShowNormal : windowPlacement.showCmd);
      var windowHandle = new WindowInteropHelper(window).Handle;
      SetWindowPlacement(windowHandle, ref windowPlacement);
    }
    catch (Exception exception)
    {
      _logger.LogWarning(exception, "Failed to set Window Position from the Settings");
    }
  }

  private static bool AreSettingsEmpty(WindowSettings settings)
  {
    return settings.Length == 0
           && settings.Flags == 0
           && settings.ShowCmd == 0
           && settings.MinPositionX == 0
           && settings.MinPositionY == 0
           && settings.MaxPositionX == 0
           && settings.MaxPositionY == 0
           && settings.NormalPositionLeft == 0
           && settings.NormalPositionTop == 0
           && settings.NormalPositionRight == 0
           && settings.NormalPositionBottom == 0;
  }

  public WindowSettings GetSettings(Window window)
  {
    // Persist window placement details to application settings
    var windowHandle = new WindowInteropHelper(window).Handle;
    GetWindowPlacement(windowHandle, out var windowPlacement);
    return GetSettings(windowPlacement);
  }

  private static WindowPlacement GetPlacement(WindowSettings settings) => new()
  {
    length = settings.Length,
    flags = settings.Flags,
    showCmd = settings.ShowCmd,
    minPosition = new Point(settings.MinPositionX, settings.MinPositionY),
    maxPosition = new Point(settings.MaxPositionX, settings.MaxPositionY),
    normalPosition = new Rect(settings.NormalPositionLeft, settings.NormalPositionTop, settings.NormalPositionRight, settings.NormalPositionBottom)
  };

  private static WindowSettings GetSettings(WindowPlacement windowPlacement) => new ()
  {
    Length = windowPlacement.length,
    Flags = windowPlacement.flags,
    ShowCmd = windowPlacement.showCmd,
    MinPositionX = windowPlacement.minPosition.X,
    MinPositionY = windowPlacement.minPosition.Y,
    MaxPositionX = windowPlacement.maxPosition.X,
    MaxPositionY = windowPlacement.maxPosition.Y,
    NormalPositionLeft = windowPlacement.normalPosition.Left,
    NormalPositionTop = windowPlacement.normalPosition.Top,
    NormalPositionRight = windowPlacement.normalPosition.Right,
    NormalPositionBottom = windowPlacement.normalPosition.Bottom
  };
}
