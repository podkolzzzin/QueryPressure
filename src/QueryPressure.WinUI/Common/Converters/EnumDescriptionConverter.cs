using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QueryPressure.WinUI.Common.Converters;

public class EnumDescriptionConverter : IValueConverter
{
  public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
  {
    return value == null ? DependencyProperty.UnsetValue : GetDescription((Enum)value);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value;
  }

  public static string GetDescription(Enum en)
  {
    var type = en.GetType();
    var memInfo = type.GetMember(en.ToString());
    if (memInfo.Length <= 0)
    {
      return en.ToString();
    }

    var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
    return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : en.ToString();
  }
}
