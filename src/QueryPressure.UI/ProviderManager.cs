using Autofac;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.UI;

public class ProviderManager
{
  private readonly Dictionary<string, Provider> _providers;

  public ProviderManager(IComponentContext container, ICreator<IConnectionProvider>[] creators)
  {
    _providers = creators.ToDictionary(x => x.Type, x => container.Resolve<Provider>(new NamedParameter("creator", x)));
  }

  public Provider GetProvider(string providerName)
  {
    return _providers[providerName.ToLower()];
  }
}
