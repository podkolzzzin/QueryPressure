import { useEffect } from 'react';
import ChartJS from 'chart.js/auto'; // import everything
import { TLineChartProps } from './LineChart.models';


export const LineChart = ({ title, data, fullWidth, fullHeight }: TLineChartProps) => {
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