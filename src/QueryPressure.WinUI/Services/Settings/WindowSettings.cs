namespace QueryPressure.WinUI.Services.Settings;

public readonly record struct WindowSettings(int Length, int Flags, int ShowCmd,
  int MinPositionX, int MinPositionY, int MaxPositionX, int MaxPositionY,
  int NormalPositionLeft, int NormalPositionTop, int NormalPositionRight, int NormalPositionBottom);
