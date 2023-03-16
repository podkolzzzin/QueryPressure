import {ProfileModel} from '@models';

export const ProfilesApi = {
  getAll(): Promise<ProfileModel[]> {
    return fetch('/api/profiles')
      .then(r => r.json());
  }
};
