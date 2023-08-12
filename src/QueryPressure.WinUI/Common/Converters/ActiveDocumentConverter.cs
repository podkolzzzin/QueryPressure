using System.Windows.Data;
using QueryPressure.WinUI.ViewModels.DockElements;

namespace QueryPressure.WinUI.Common.Converters;

public class ActiveDocumentConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
    return value is DocumentViewModel ? value : Binding.DoNothing;
  }

  public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
    return value is DocumentViewModel ? value : Binding.DoNothing;
  }
}
