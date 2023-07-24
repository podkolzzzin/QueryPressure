export type TMonitoringScreenProps = {
    executionId: string;
    showMonitor: boolean;
    toggleMonitor: () => void;
    toggleCancelButton: (enabled?: boolean) => void;
};
