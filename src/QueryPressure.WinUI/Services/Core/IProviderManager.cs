namespace QueryPressure.WinUI.Services.Core;

public interface IProviderManager
{
  IProvider GetProvider(string providerName);
}
