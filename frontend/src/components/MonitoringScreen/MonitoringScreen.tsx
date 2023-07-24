import { useEffect, useState } from 'react';
import { TMonitoringScreenProps } from "./MonitoringScreen.models";
import { Arrow } from "../Arrow";
import { LineChart } from '../LineChart';
import { EventHandlerSet, subsctibeToExecutionEvents } from '@/services';

export const MonitoringScreen = ({ executionId, showMonitor, toggleMonitor, toggleCancelButton }: TMonitoringScreenProps) => {
    const [fullView, setFullView] = useState(false);
    
    const [requestCountMetric, setRequestCountMetric] = useState<number>(0); // Initial metric value
    const [averageMetric, setAverageMetric] = useState<number>(0); // Initial metric value

    const [requestCountChartData, setRequestCountChartData] = useState<{ x: number, y: number }[]>([]);
    const [averageChartData, setAverageChartData] = useState<{ x: number, y: number }[]>([]);

    const eventHandlers: EventHandlerSet = {
        requestCountMetricReceived: setRequestCountMetric,
        averageMetricReceived: setAverageMetric,
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
        subsctibeToExecutionEvents(executionId, eventHandlers);
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
                    <LineChart title='Average' fullWidth={fullView} fullHeight={fullView} data={averageChartData} />
                    <LineChart title='Request Count' fullWidth={fullView} fullHeight={fullView} data={requestCountChartData} />
                    {/* <Chart title='RPS / Error rate' fullWidth fullHeight={fullView} /> */}
                </div>
            }
        </div>);
}