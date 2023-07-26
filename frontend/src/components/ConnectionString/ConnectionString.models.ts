import {ValidationMessage} from '@models/ValidationMessage';

export interface ConnectionStringProps {
  initialValue: string;
  validationMessage: ValidationMessage | null;
  changed: (connectionString: string) => void;
  test: () => void;
}
