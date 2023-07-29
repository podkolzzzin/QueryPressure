import { ProviderModel } from "@/models";

export interface EditorCardProps {
  providers: string[];
  executionId: string;
  selectedProvider: string | null;
  selectedProviderInfo: ProviderModel | null;
  selectProvider: (provider: string) => void;
  setScript: (script: string) => void;
  theme: string;
  showMonitor: boolean;
  toggleMonitor: () => void;
  toggleCancelButton: (enabled?: boolean) => void;
}
