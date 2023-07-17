export interface EditorCardProps {
  providers: string[];
  selectedProvider: string | null;
  selectProvider: (provider: string) => void;
  setScript: (script: string) => void;
  theme: string;
  showMonitor: boolean;
  toggleMonitor: () => void;
}
