import {ProviderModel} from '@models';

export const ProvidersApi = {
  getAll(): Promise<ProviderModel[]> {
    return fetch('/api/providers')
      .then(r => r.json());
  }
};
