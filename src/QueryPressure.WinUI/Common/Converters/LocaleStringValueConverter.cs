using System.Globalization;
using System.Windows.Data;
using QueryPressure.WinUI.ViewModels.Helpers;

namespace QueryPressure.WinUI.Common.Converters;

public class LocaleStringValueConverter : IMultiValueConverter
{
  private readonly LocaleViewModel? _viewModel;

  public LocaleStringValueConverter(LocaleViewModel? viewModel)
  {
    _viewModel = viewModel;
  }

  public object Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
  {
    if (_viewModel == null)
    {
      throw new InvalidOperationException("Locale ViewModel is null");
    }

    if (values.Length < 2)
    {
      throw new ArgumentException(nameof(values));
    }

    var key = values[0]?.ToString() ?? string.Empty;
    var currentLocale = values[1]?.ToString() ?? string.Empty;

    if (currentLocale != _viewModel.CurrentLanguage)
    {
      throw new InvalidOperationException("Provided Locale not equal ViewModel.Locale");
    }

    var stringFormat = parameter?.ToString();

    if (!string.IsNullOrEmpty(stringFormat))
    {
      key = string.Format(stringFormat, key);
    } 

    return _viewModel.Strings.TryGetValue(key, out var str) ? str : $"<!- {key} -!>";
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
