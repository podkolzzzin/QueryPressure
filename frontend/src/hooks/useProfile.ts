import { useEffect, useState } from 'react';
import { ProfilesApi } from '@api';
import { ProfileModel } from '@models';

import { UrlService } from '@/services/UrlService';

export function useProfile() {
  const [selectedProfile, setSelectedProfile] = useState<ProfileModel | null>();
  const [profiles, setProfiles] = useState<ProfileModel[]>([]);

  function selectProfile(profileType: string) {
    const profile: ProfileModel = profiles.find(p => p.type === profileType)!;
    setSelectedProfile(profile);
    UrlService.set('profile', profileType);
  }

  useEffect(() => {
    function loadProfiles(): void {
      ProfilesApi
        .getAll()
        .then(profiles => {
          setProfiles(profiles);
          // After the profiles have been loaded, check if a 'profile' parameter exists in the URL
          const urlProfileType = UrlService.get('profile');
          // If it does exist, select the profile with that type
          if (urlProfileType) {
            selectProfile(urlProfileType);
          }
        });
    }
    loadProfiles();
  });

  return {
    profiles,
    selectProfile,
    selectedProfile
  };
}
