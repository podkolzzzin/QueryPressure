using System.Windows.Input;
using Autofac.Core;
using Microsoft.Extensions.DependencyInjection;
using QueryPressure.WinUI.Common.Commands;

namespace QueryPressure.WinUI.Common.Extensions;

public class CommandImportExtension : BaseMarkupExtension
{
  private readonly Type _serviceType;

  public CommandImportExtension(Type serviceType)
  {
    _serviceType = serviceType;
  }

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    if (!_serviceType.IsAssignableTo(typeof(ICommand)))
    {
      throw new ArgumentException($"Failed to get ICommand from the '{_serviceType.Name}'");
    }

    serviceProvider = ServiceProvider ?? serviceProvider;

    try
    {
      return serviceProvider.GetRequiredService(_serviceType);
    }
    catch (DependencyResolutionException)
    {
      throw;
    }
    catch
    {
      return new DelegateCommand<object>(_ => { });
    }
  }
}
