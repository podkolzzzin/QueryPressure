import {ProfilesApi} from "@api";
import {ProfileModel} from "@models";
import {useEffect, useState} from "react";

export function useProfile() {
  const [selectedProfile, setSelectedProfile] = useState<ProfileModel | null>(null);
  const [profiles, setProfiles] = useState<ProfileModel[]>([]);

  function selectProfile(profileType: string) {
    const profile: ProfileModel = profiles.find(p => p.type === profileType)!;
    setSelectedProfile(profile);
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
