using System.Windows.Data;
using QueryPressure.WinUI.ViewModels;

namespace QueryPressure.WinUI.Common.Converters;

public class ActiveDocumentConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
    return value is ScriptViewModel ? value : Binding.DoNothing;
  }

  public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  {
    return value is ScriptViewModel ? value : Binding.DoNothing;
  }
}
