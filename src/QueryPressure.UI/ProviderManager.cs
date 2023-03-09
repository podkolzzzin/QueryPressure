using Autofac;
using Autofac.Core;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;

public class ProviderManager
{
  private readonly Dictionary<string, Provider> _providers;
  
  public ProviderManager(IContainer container, ICreator<IConnectionProvider>[] creators)
  {
    _providers = creators.ToDictionary(x => x.Type, x => container.Resolve<Provider>(new NamedParameter("creator", x)));
  }
  
  public Provider GetProvider(string providerName)
  {
    return _providers[providerName];
  }
}
