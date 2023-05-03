using System.Windows;
using System.Windows.Data;
using Microsoft.Extensions.DependencyInjection;
using QueryPressure.WinUI.Common.Converters;
using QueryPressure.WinUI.ViewModels.Helpers;

namespace QueryPressure.WinUI.Common.Extensions;

public class LocaleExtension : BaseMarkupExtension
{
  private readonly LocaleViewModel? _viewModel;
  private readonly string? _key;
  private readonly LocaleStringValueConverter _converter;

  public Binding? Binding { get; set; }

  public LocaleExtension() : this(null)
  {
  }

  public LocaleExtension(string? key)
  {
    _key = key;
    _viewModel = ServiceProvider?.GetRequiredService<LocaleViewModel>();
    _converter = new LocaleStringValueConverter(_viewModel);
  }

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    if (_viewModel == null)
    {
      return string.IsNullOrEmpty(_key) ? "en-US(dev)" : _key;
    }

    var bindingCurrentLanguage = new Binding
    {
      Source = _viewModel,
      Mode = BindingMode.OneWay,
      Path = new PropertyPath("CurrentLanguage"),
    };

    if (string.IsNullOrEmpty(_key) && Binding == null)
    {
      return bindingCurrentLanguage;
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
      Bindings = { firstBinding, bindingCurrentLanguage },
      Converter = _converter,
      ConverterParameter = Binding?.StringFormat
    };

    return multiBindingResult.ProvideValue(serviceProvider);
  }
}
