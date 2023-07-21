import React, { useEffect, useRef, useState } from 'react';
import { TMonitoringScreenProps } from "./MonitoringScreen.models";
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { Arrow } from "../Arrow";
import ChartJS from 'chart.js/auto'; // import everything

let connection: HubConnection | null = null;

type EventHandlerSet = {
    setRequestCountMetric: (value: number) => void,
    setAverageMetric: (value: number) => void,
    notifyExecutionCompleted: (isSuccessful: boolean, message: string | null) => void;
    newConnectionEstablished: () => void,
};

type TChartProps = { 
    title: string, 
    data: { x: number, y: number }[],
    fullWidth?: boolean, 
    fullHeight?: boolean 
}

const setupConnection = (id: string, handlers: EventHandlerSet) => {
    // resubscribe if connection exist
    if (connection) connection.stop();

    connection = new HubConnectionBuilder()
        .withUrl(`/ws/dashboard?executionId=${id}`)
        .withAutomaticReconnect()
        .build();

    connection.on('live-average', (metric) => handlers.setAverageMetric(metric.nanoseconds));
    connection.on('live-request-count', handlers.setRequestCountMetric);
    connection.on('execution-completed', ({ isSuccessful, message }) =>
        handlers.notifyExecutionCompleted(isSuccessful, message));

    connection.start()
        .then(() => {
            console.log('SignalR connection established');
            handlers.newConnectionEstablished();
        })
        .catch((error) => {
            console.error('SignalR connection error:', error);
        });
};

const Chart = ({ title, data, fullWidth, fullHeight }: TChartProps) => {
    const id = `myChart-${title}`
    useEffect(() => {
        const chart = new ChartJS(id, {
            type: 'line',
            data: {
                labels: data.map(x => x.x),
                datasets: [{
                    label: title,
                    data: data.map(x => x.y),
                    fill: false,
                    borderColor: 'rgb(75, 192, 192)',
                    tension: 0
                }]
            },
            options: {
                animation: {
                    duration: 0
                },
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                    },
                    x: {
                        display: false,
                    }
                },
            }
        });
        return () => {
            chart.destroy()
        }
    }, [data])

    return (<div className={`card ${fullHeight && 'h-100'}`} style={{ width: fullWidth ? '100%' : '49%' }}>
        <div className="card-header">{title}</div>
        <div className="card-body d-flex justify-content-center">
            <canvas id={id}></canvas>
        </div>
    </div>)
};

export const MonitoringScreen = ({ executionId, showMonitor, toggleMonitor, toggleCancelButton }: TMonitoringScreenProps) => {
    const [fullView, setFullView] = useState(false);
    
    const [requestCountMetric, setRequestCountMetric] = useState<number>(0); // Initial metric value
    const [averageMetric, setAverageMetric] = useState<number>(0); // Initial metric value

    const [requestCountChartData, setRequestCountChartData] = useState<{ x: number, y: number }[]>([]);
    const [averageChartData, setAverageChartData] = useState<{ x: number, y: number }[]>([]);

    const eventHandlers: EventHandlerSet = {
        setRequestCountMetric: setRequestCountMetric,
        setAverageMetric: setAverageMetric,
        notifyExecutionCompleted: (isSuccessful: boolean, message: string | null) => { 
            toggleCancelButton(false);
        },
        newConnectionEstablished: () => {
            setRequestCountChartData([]);
            setAverageChartData([]);
            toggleCancelButton(true);
        }
    }

    useEffect(() => {
        if (!executionId) return;
        setupConnection(executionId, eventHandlers);
    }, [executionId]);


    useEffect(() => {
        setRequestCountChartData([...requestCountChartData, { y: requestCountMetric, x: requestCountChartData.length }])
    }, [requestCountMetric]);

    useEffect(() => {
        setAverageChartData([...averageChartData, { y: averageMetric, x: averageChartData.length }])
    }, [averageMetric]);

    const onTop = () => {
        if (!showMonitor) {
            toggleMonitor();
        } else {
            setFullView(true);
        }
    };

    const onBottom = () => {
        if (fullView) {
            setFullView(false);
        } else {
            toggleMonitor();
        }
    }

    return (
        <div className='card position-absolute' style={{ height: fullView && '90%' || showMonitor && '50%' || '', width: 'calc(100% - 28px)', bottom: '60px' }}>
            <div className="d-flex" style={{ height: '32px', justifyContent: 'center', alignItems: 'center' }}>
                <Arrow onClick={onTop} disabled={showMonitor && fullView} />
                <Arrow onClick={onBottom} disabled={!showMonitor} down />
            </div>
            {showMonitor &&
                <div className={`d-flex h-100 ${!fullView ? 'flex-wrap' : 'flex-column'}`} style={{ gap: '2%', margin: '0 10px 10px' }}>
                    <Chart title='Average' fullWidth={fullView} fullHeight={fullView} data={averageChartData} />
                    <Chart title='Request Count' fullWidth={fullView} fullHeight={fullView} data={requestCountChartData} />
                    {/* <Chart title='RPS / Error rate' fullWidth fullHeight={fullView} /> */}
                </div>
            }
        </div>);
}