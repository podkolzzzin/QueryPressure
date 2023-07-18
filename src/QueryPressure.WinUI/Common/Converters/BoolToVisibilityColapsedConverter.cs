using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace QueryPressure.WinUI.Common.Converters;

[ValueConversion(typeof(bool), typeof(Visibility))]
public sealed class BoolToVisibilityColapsedConverter : IValueConverter
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
    var isInverted = System.Convert.ToBoolean(parameter);
    var isVisible = value != null && (bool)value;
    if (isVisible)
      return isInverted ? Visibility.Collapsed : Visibility.Visible;
    else
      return isInverted ? Visibility.Visible : Visibility.Collapsed;
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
    var visibility = value == null ? Visibility.Collapsed : (Visibility)value;
    var isInverted = System.Convert.ToBoolean(parameter);

    return (visibility == Visibility.Visible) != isInverted;
  }
}
