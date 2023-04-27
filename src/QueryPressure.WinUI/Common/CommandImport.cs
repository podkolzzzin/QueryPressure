using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.Extensions.DependencyInjection;
using QueryPressure.WinUI.Common.Commands;

namespace QueryPressure.WinUI.Common;

public class CommandImport : MarkupExtension
{
  private readonly Type _serviceType;

  public CommandImport(Type serviceType)
  {
    _serviceType = serviceType;
  }

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    if (_serviceType.IsAssignableTo(typeof(ICommand)))
    {
      var currentApplication = Application.Current;

      if (currentApplication is App app)
      {
        return app.ServiceProvider.GetRequiredService(_serviceType);

      }

      return new DelegateCommand<object>(_ => { });
    }

    throw new ArgumentException($"Failed to get ICommand from the '{_serviceType.Name}'");
  }
}
