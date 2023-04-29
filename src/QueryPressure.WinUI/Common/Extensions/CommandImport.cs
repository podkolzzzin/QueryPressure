using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using QueryPressure.WinUI.Common.Commands;

namespace QueryPressure.WinUI.Common.Extensions;

public class CommandImport : BaseMarkupExtension
{
  private readonly Type _serviceType;

  public CommandImport(Type serviceType)
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
    catch
    {
      return new DelegateCommand<object>(_ => { });
    }
  }
}
