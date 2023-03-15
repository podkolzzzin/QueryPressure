import {LimitModel} from '@models';

export const LimitsApi = {
  getAll(): Promise<LimitModel[]> {
    return fetch('/api/limits')
      .then(r => r.json());
  }
};
