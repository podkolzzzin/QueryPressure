using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Microsoft.Extensions.DependencyInjection;
using QueryPressure.WinUI.ViewModels.Helpers;

namespace QueryPressure.WinUI.Common.Extensions;

public class LocaleExtension : BaseMarkupExtension
{
  private readonly LocaleViewModel? _viewModel;
  private readonly string? _key;

  public Binding? Binding { get; set; }

  public LocaleExtension() : this(null)
  {
  }

  public LocaleExtension(string? key)
  {
    _key = key;
    _viewModel = ServiceProvider?.GetRequiredService<LocaleViewModel>();
  }

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    if (_viewModel == null)
    {
      return string.IsNullOrEmpty(_key) ? "en-US(dev)" : _key;
    }

    var key = _key;

    if (string.IsNullOrEmpty(key))
    {
      if (Binding == null) 
      {
        var bindingCurrentLanguage = new Binding
        {
          Source = _viewModel,
          Mode = BindingMode.OneWay,
          Path = new PropertyPath("CurrentLanguage"),
        };

        return bindingCurrentLanguage.ProvideValue(serviceProvider);
      }
      else
      {
        var provideValueTarget = serviceProvider.GetRequiredService<IProvideValueTarget>();

        if (provideValueTarget.TargetObject.GetType().FullName == "System.Windows.SharedDp")
        {
          // In a control template the TargetObject is a SharedDp (internal WPF class)
          // In that case, the markup extension itself is returned to be re-evaluated later in the context where the template is applied
          return this;
        }

        var value = Binding.ProvideValue(serviceProvider);
        var bindingExpression = value as BindingExpression;

        if (bindingExpression == null)
        {
          throw new InvalidOperationException("The binding must return a BindingExpressionBase.");
        }

        if (provideValueTarget.TargetObject is not FrameworkElement element)
        {
          throw new InvalidOperationException("The MarkupExtension can only be used on a FrameworkElement.");
        }

        var boundKey = EvaluateBindingExpression(element, bindingExpression)?.ToString();

        if (!string.IsNullOrEmpty(boundKey))
        {
          key = boundKey;
        }
      }
    } 
    
    if (!string.IsNullOrEmpty(key) && !_viewModel.Strings.ContainsKey(key))
    {
      return key;
    }

    var bindingResult = new Binding
    {
      Source = _viewModel,
      Mode = BindingMode.OneWay,
      Path = new PropertyPath($"Strings[{key}]"),
    };

    return bindingResult.ProvideValue(serviceProvider);
  }

  private static object? EvaluateBindingExpression(FrameworkElement element, BindingExpression bindingExpression)
  {
    var format = bindingExpression.ParentBinding.StringFormat;

    if (bindingExpression.ParentBinding.Path != null)
    {
      return GetValue(element.DataContext.GetType()
        .GetProperty(bindingExpression.ParentBinding.Path.Path)?
        .GetValue(element.DataContext)?
        .ToString(), format);
    }

    return GetConvertedValue(bindingExpression, GetValue(element.DataContext, format));

  }

  private static object? GetConvertedValue(BindingExpression bindingExpression, object? value)
  {
    if (bindingExpression.ParentBinding.Converter != null)
    {
      return bindingExpression.ParentBinding.Converter
        .Convert(value, typeof(string), bindingExpression.ParentBinding.ConverterParameter, CultureInfo.CurrentUICulture);
    }

    return value;
  }

  private static object? GetValue(object? value, string format)
  {
    return string.IsNullOrEmpty(format) ? value : string.Format(format, value);
  }
}
