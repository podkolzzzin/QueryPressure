using Microsoft.Extensions.DependencyInjection;
using QueryPressure.WinUI.ViewModels;
using System.Windows.Data;
using System.Windows;

namespace QueryPressure.WinUI.Common.Extensions
{
  internal class ThemeExtension : BaseMarkupExtension
  {
    private readonly ThemeViewModel? _viewModel;

    public ThemeExtension()
    {
      _viewModel = ServiceProvider?.GetRequiredService<ThemeViewModel>();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      var bindingResult = new Binding
      {
        Source = _viewModel,
        Mode = BindingMode.OneWay,
        Path = new PropertyPath("CurrentTheme"),
      };

      return bindingResult.ProvideValue(serviceProvider);
    }
  }
}
