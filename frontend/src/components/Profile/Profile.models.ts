import {ProfileModel} from "@models/ProfileModel";

export interface ProfileProps {
  profiles: ProfileModel[];
  selectedProfile: ProfileModel | null
  selectProfile: (profileType: string) => void;
}
