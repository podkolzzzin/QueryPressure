import {LimitModel} from "@models/LimitModel";

export interface LimitProps {
  limits: LimitModel[];
  selectedLimit: LimitModel | null;
  selectLimit: (limitType: string) => void;
}
