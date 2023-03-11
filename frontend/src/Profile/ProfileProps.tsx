import {ProfileModel} from "../models/ProfileModel";

export interface ProfileProps {
  profiles: ProfileModel[];
  selectedProfile: ProfileModel;
  selectProfile: (profileType: string) => void;
}
