using QueryPressure.WinUI.Dtos;
using System.Globalization;
using System.Windows.Data;

namespace QueryPressure.WinUI.Converters;

public class ProviderConnectionStringConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values.Length != 2)
    {
      throw new ArgumentException(nameof(values));
    }

    var provider = values[0].ToString() ?? string.Empty;
    var connectionString = values[1].ToString() ?? string.Empty;

    return new TestConnectionStringDto(provider, connectionString);
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
