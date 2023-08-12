using System.Collections.Immutable;
using Autofac;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.UI;

internal class ProviderManager
{
  private readonly ImmutableDictionary<string, Provider> _providers;

  public ProviderManager(IComponentContext container, ICreator<IConnectionProvider>[] creators)
  {
    _providers = creators.ToImmutableDictionary(x => x.Type, x => container.Resolve<Provider>(new NamedParameter("creator", x)));
  }

  public Provider GetProvider(string providerName)
  {
    return _providers[providerName.ToLower()];
  }

  public Provider? GetProvider(Guid executionId) => FindProvider(executionId)?.Provider;

  public Execution? GetExecution(Guid executionId) => FindProvider(executionId)?.Execution;

  private (Execution Execution, Provider Provider, string ProviderName)? FindProvider(Guid executionId)
  {
    foreach (var (providerName, provider) in _providers)
    {
      var execution = provider.GetExecution(executionId);
      if (execution != null)
      {
        return (execution, provider, providerName);
      }
    }
    return null;
  }
}
