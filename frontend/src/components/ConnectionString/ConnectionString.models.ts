export interface ValidationMessage {
  isGood: boolean;
  message: string;
}

export interface ConnectionStringProps {
  validationMessage: ValidationMessage | null;
  changed: (connectionString: string) => void;
  test: () => void;
}
