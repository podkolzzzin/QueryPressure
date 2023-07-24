export interface ConfigurationCardProps {
  selectedProvider: string | null;
  script: string;
  executionId: string,
  cancelButtonEnabled: boolean;
  toggleTheme: () => void;
  openMonitoring: () => void;
  setExecutionId: (executionId: string) => void;
}
