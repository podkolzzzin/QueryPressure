namespace QueryPressure.WinUI.Services.Settings;

public record WindowSettings(int Length = 0, int Flags = 0, int ShowCmd = 0,
  int MinPositionX = 0, int MinPositionY = 0, int MaxPositionX = 0, int MaxPositionY = 0,
  int NormalPositionLeft = 0, int NormalPositionTop = 0, int NormalPositionRight = 0, int NormalPositionBottom = 0);
