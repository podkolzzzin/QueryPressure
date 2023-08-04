import { useEffect, useState } from 'react';
import { ProfilesApi } from '@api';
import { ProfileModel } from '@models';

import { UrlService } from '@/services/UrlService';

export function useProfile() {
  const [selectedProfile, setSelectedProfile] = useState<ProfileModel | null>(null);
  const [profiles, setProfiles] = useState<ProfileModel[]>([]);

  function selectProfile(profileType: string) {
    const profile: ProfileModel = profiles.find(p => p.type === profileType)!;
    setSelectedProfile(profile);
    UrlService.set('profile', profileType);
  }

  function loadProfiles(): void {
    ProfilesApi
      .getAll()
      .then(profiles => setProfiles(profiles));
  }

  useEffect(() => {
    loadProfiles();
  }, [setProfiles]);

  return {
    profiles,
    selectProfile,
    selectedProfile
  };
}
