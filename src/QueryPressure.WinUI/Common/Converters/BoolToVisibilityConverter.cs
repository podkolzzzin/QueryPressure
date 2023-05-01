using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace QueryPressure.WinUI.Common.Converters;

[ValueConversion(typeof(bool), typeof(Visibility))]
public sealed class BoolToVisibilityConverter : IValueConverter
{
  /// <summary>
  /// Converts a <seealso cref="Boolean"/> value
  /// into a <seealso cref="Visibility"/> value.
  /// </summary>
  /// <param name="value"></param>
  /// <param name="targetType"></param>
  /// <param name="parameter"></param>
  /// <param name="culture"></param>
  /// <returns></returns>
  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
  {
    bool isInverted = parameter != null && (bool)parameter;
    var isVisible = value != null && (bool)value;
    if (isVisible)
      return isInverted ? Visibility.Hidden : Visibility.Visible;
    else
      return isInverted ? Visibility.Visible : Visibility.Hidden;
  }

  /// <summary>
  /// Converts a <seealso cref="Visibility"/> value
  /// into a <seealso cref="Boolean"/> value.
  /// </summary>
  /// <param name="value"></param>
  /// <param name="targetType"></param>
  /// <param name="parameter"></param>
  /// <param name="culture"></param>
  /// <returns></returns>
  public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
  {
    var visibility = value == null ? Visibility.Hidden : (Visibility)value;
    var isInverted = parameter != null && (bool)parameter;

    return (visibility == Visibility.Visible) != isInverted;
  }
}
