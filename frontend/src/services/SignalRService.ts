import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';

let connection: HubConnection | null = null;

export const subsctibeToExecutionEvents = (executionId: string, handlers: EventHandlerSet) => {
    // resubscribe if connection exist
    if (connection) connection.stop();

    connection = new HubConnectionBuilder()
        .withUrl(`/ws/dashboard?executionId=${executionId}`)
        .withAutomaticReconnect()
        .build();

    connection.on('live-average', (metric) => handlers.averageMetricReceived(metric.nanoseconds));
    connection.on('live-request-count', handlers.requestCountMetricReceived);
    connection.on('execution-completed', ({ isSuccessful, message }) => {
        handlers.notifyExecutionCompleted(isSuccessful, message);
        
        // 'execution-completed' is the last event that user should receive from server 
        connection?.stop();
        connection = null;
    });

    connection.start()
        .then(() => {
            console.log('SignalR connection established');
            handlers.newConnectionEstablished();
        })
        .catch((error) => {
            console.error('SignalR connection error:', error);
        });
};

export type EventHandlerSet = {
    requestCountMetricReceived: (value: number) => void,
    averageMetricReceived: (value: number) => void,
    notifyExecutionCompleted: (isSuccessful: boolean, message: string | null) => void;
    newConnectionEstablished: () => void,
};