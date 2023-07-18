export interface StatusBarProps {
    status: string;
    providers: string[];
    selectedProvider: string | null;
    selectProvider: (provider: string) => void;
    handleFileUpload: (file: any) => void;
    allowedFileTypes: string[];
}
