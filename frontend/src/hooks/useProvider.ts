import {useEffect, useState} from "react";
import {ProviderService} from "@services";
import {ProvidersApi} from "@api";

export function useProvider() {
  const [selectedProvider, setSelectedProvider] = useState<string | null>(null);
  const [providers, setProviders] = useState<string[]>([]);

  function selectProvider(provider: string) {
    setSelectedProvider(provider);
    ProviderService.saveCurrent(provider);
  }

  function loadProviders(): void {
    ProvidersApi
      .getAll()
      .then(providers => setProviders(providers));

    const currentProvider = ProviderService.getCurrent();
    setSelectedProvider(currentProvider);
  }


  useEffect(() => {
    loadProviders();
  }, []);

  return {
    providers,
    selectedProvider,
    selectProvider
  };
}
