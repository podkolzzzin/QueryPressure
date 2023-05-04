using System.Windows;
using System.Windows.Data;
using Microsoft.Extensions.DependencyInjection;
using QueryPressure.WinUI.Common.Converters;
using QueryPressure.WinUI.ViewModels.Helpers;

namespace QueryPressure.WinUI.Common.Extensions;

public class LocaleExtension : BaseMarkupExtension
{
  private readonly LocaleViewModel? _viewModel;
  private readonly LocaleStringValueConverter _converter;
  private readonly Binding _currentLanguageBinding;
  private readonly string? _key;

  public Binding? Binding { get; set; }

  public LocaleExtension() : this(null)
  {
  }

  public LocaleExtension(string? key)
  {
    _key = key;
    _viewModel = ServiceProvider?.GetRequiredService<LocaleViewModel>();
    _converter = ServiceProvider?.GetRequiredService<LocaleStringValueConverter>() ?? new LocaleStringValueConverter(_viewModel);
    _currentLanguageBinding = new Binding
    {
      Source = _viewModel,
      Mode = BindingMode.OneWay,
      Path = new PropertyPath("CurrentLanguage"),
    };
  }

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    if (_viewModel == null)
    {
      return new Binding(".")
      {
        Source = string.IsNullOrEmpty(_key) ? "en-US(dev)" : _key,
      };
    }

    if (string.IsNullOrEmpty(_key) && Binding == null)
    {
      return _currentLanguageBinding;
    }

    var firstBinding = string.IsNullOrEmpty(_key) && Binding != null
      ? Binding
      : new Binding
      {
        Source = _key,
        Mode = BindingMode.OneWay,
        Path = new PropertyPath("."),
      };

    var multiBindingResult = new MultiBinding()
    {
      Bindings = { firstBinding, _currentLanguageBinding },
      Converter = _converter,
      ConverterParameter = Binding?.StringFormat
    };

    return multiBindingResult.ProvideValue(serviceProvider);
  }
}
