import { HubConnection,HubConnectionBuilder } from '@microsoft/signalr';

import { MetricNames } from '@/models/MetricModel';
import { MetricsEvent } from '@/models/MetricsEvent';

let connection: HubConnection | null = null;

export const subsctibeToExecutionEvents = (executionId: string, handlers: EventHandlerSet) => {
    // resubscribe if connection exist
    if (connection) connection.stop();

    connection = new HubConnectionBuilder()
        .withUrl(`/ws/dashboard?executionId=${executionId}`)
        .withAutomaticReconnect()
        .build();

    connection.on('live-metrics', (metricsEvent: MetricsEvent) => {
        metricsEvent.metrics.forEach((metric) => {
            switch (metric.name) {
                case MetricNames.LiveAverage: 
                    handlers.averageMetricReceived(metric.value.nanoseconds);
                    break;
                case MetricNames.LiveRequestCount:
                    handlers.requestCountMetricReceived(metric.value);
            }
        });
    });

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