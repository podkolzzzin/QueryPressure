using System.Windows;
using System.Windows.Data;
using Microsoft.Extensions.DependencyInjection;
using QueryPressure.WinUI.ViewModels;

namespace QueryPressure.WinUI.Common.Extensions;

public class Locale : BaseMarkupExtension
{
  private readonly LocaleViewModel? _viewModel;
  private readonly string _key;

  public Locale() : this(string.Empty)
  {
  }

  public Locale(string key)
  {
    _key = key;
    _viewModel = ServiceProvider?.GetRequiredService<LocaleViewModel>();
  }

  public override object ProvideValue(IServiceProvider serviceProvider)
  {

    if (_viewModel == null)
    {
      return _key;
    }

    var binding = new Binding
    {
      Source = _viewModel,
      Mode = BindingMode.OneWay,
      Path = string.IsNullOrEmpty(_key) ? new PropertyPath("CurrentLanguage") : new PropertyPath($"Strings[{_key}]")
    };

    return binding.ProvideValue(serviceProvider);
  }
}
