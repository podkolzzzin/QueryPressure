import {useEffect, useState} from 'react';
import {ProvidersApi} from '@api';
import {ProviderService} from '@services';
import { ProviderModel } from '@/models';

export function useProvider() {
  const [providerMap, setProviderMap] = useState<Map<string, ProviderModel> | null>(null);
  const [selectedProviderInfo, setSelectedProviderInfo] = useState<ProviderModel | null>(null);
  const [selectedProvider, setSelectedProvider] = useState<string | null>(null);
  const [providers, setProviders] = useState<string[]>([]);

  function selectProvider(provider: string) {
    setSelectedProvider(provider);
    setSelectedProviderInfo(providerMap?.get(provider) ?? null);
    ProviderService.saveCurrent(provider);
  }

  function loadProviders(): void {
    ProvidersApi
      .getAll()
      .then(providers => {
        const providerMap = new Map(providers.map(x => [x.name, x]));
        const currentProvider = ProviderService.getCurrent();
        setProviderMap(providerMap);
        setSelectedProvider(currentProvider);
        currentProvider && setSelectedProviderInfo(providerMap.get(currentProvider) ?? null);
      });
  }

  useEffect(() => {
    loadProviders();
  }, []);

  useEffect(() => {
    providerMap && setProviders([...providerMap.keys()]);
  }, [providerMap]);

  return {
    providers,
    selectedProvider,
    selectedProviderInfo,
    selectProvider
  };
}
