import {LimitModel} from "../models/LimitModel";

export interface LimitProps {
  limits: LimitModel[];
  selectedLimit: LimitModel;
  selectLimit: (limitType: string) => void;
}
