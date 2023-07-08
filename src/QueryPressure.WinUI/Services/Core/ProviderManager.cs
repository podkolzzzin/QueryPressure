using Autofac;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.WinUI.Services.Core;

public class ProviderManager : IProviderManager
{
  private readonly Dictionary<string, IProvider> _providers;

  public ProviderManager(IComponentContext container, ICreator<IConnectionProvider>[] creators)
  {
    _providers = creators.ToDictionary(x => x.Type, x => container.Resolve<IProvider>(new NamedParameter("creator", x)));
  }

  public IProvider GetProvider(string providerName)
  {
    return _providers[providerName.ToLower()];
  }
}
