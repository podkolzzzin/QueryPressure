import React, { useEffect, useRef, useState } from 'react';
import { useParams } from 'react-router-dom';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { Chart, Legend, Tooltip } from 'chart.js';
import { Chart as ChartJS, LinearScale, LineController, LineElement, PointElement, Title } from 'chart.js';
import { LineChart, CartesianGrid, XAxis, YAxis, Line } from 'recharts';

ChartJS.register(LineController, LineElement, PointElement, LinearScale, Title);

const setupConnection = (id: string, setMetric: (total: number) => void) => {
    const connection = new HubConnectionBuilder()
        .withUrl(`/ws/dashboard?executionId=${id}`)
        .build();

    connection.on('live-metrics', (metric) => {
        setMetric(metric.metrics.filter((x: any) => x.name === 'live-request-count')[0].value);
    });


    connection.start()
        .then(() => {
            console.log('SignalR connection established');
        })
        .catch((error) => {
            console.error('SignalR connection error:', error);
        });
};

export const ExecutionPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [metric, setMetric] = useState<number>(0); // Initial metric value
    const [chartData, setChartData] = useState<{ x: number, y: number }[]>([]);

    useEffect(() => {
        if (!id) return;
        setupConnection(id, (m) => {
            setMetric(m);
            setChartData([...chartData, { y: m, x: chartData.length }]);
        });
    }, [chartData, id]);

    return (
        <>
            <div>
                <h1>Execution Page</h1>
                <p>ID: {id}</p>
                {/* Other content */}
            </div>
            <div>
                <p>Executed {metric}</p>
            </div>
            <div>
                <LineChart
                    width={500}
                    height={300}
                    data={chartData}
                    margin={{
                        top: 5, right: 30, left: 20, bottom: 5,
                    }}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="name" />
                    <YAxis />
                    <Line type="monotone" dataKey="value" stroke="#8884d8" activeDot={{ r: 8 }} />
                </LineChart>
            </div>
        </>
    );
};