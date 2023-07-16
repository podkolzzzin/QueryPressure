export interface ConfigurationCardProps {
  selectedProvider: string | null;
  script: string;
  toggleTheme: () => void;
  openMonitoring: () => void;
}
