export interface ValidationMessage {
  isGood: boolean;
  message: string;
}

export interface ConnectionStringProps {
  validationMessage: ValidationMessage;
  changed: (connectionString: string) => void;
  test: () => void;
}
