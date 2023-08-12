using System.Globalization;
using System.Windows.Data;

namespace QueryPressure.WinUI.Common.Converters;

public class EqualsConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    return values.Distinct().Count() == 1;
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
