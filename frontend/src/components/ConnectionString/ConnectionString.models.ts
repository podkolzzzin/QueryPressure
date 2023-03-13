import {ValidationMessage} from "@models/ValidationMessage";

export interface ConnectionStringProps {
  validationMessage: ValidationMessage | null;
  changed: (connectionString: string) => void;
  test: () => void;
}
