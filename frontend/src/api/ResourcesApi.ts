import { ResourceModel } from '@/models';

export const ResourcesApi = {
    getAll(locale: string): Promise<ResourceModel> {
      return fetch(`/api/resources/${locale}`)
        .then(r => r.json());
    }
  };
  