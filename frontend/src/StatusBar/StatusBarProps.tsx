export interface StatusBarProps {
    status: string;
    providers: string[];
    selectProvider: (provider: string) => void;
}
