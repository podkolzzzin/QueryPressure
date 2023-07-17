import { useState } from "react";
import { TMonitoringScreenProps } from "./MonitoringScreen.models"
import { Arrow } from "../Arrow";


const Chart = ({ title, fullWidth, fullHeight }: { title: string, fullWidth?: boolean, fullHeight?: boolean }) => {
    return (<div className={`card ${fullHeight && 'h-100'}`} style={{ width: fullWidth ? '100%' : '49%' }}>
        <div className="card-header">{title}</div>
        <div className="card-body"></div>
    </div>)
};

export const MonitoringScreen = ({ showMonitor, toggleMonitor }: TMonitoringScreenProps) => {
    const [fullView, setFullView] = useState(false);

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
                    <Chart title='Average' fullWidth={fullView} fullHeight={fullView} />
                    <Chart title='Q1 / Med / Q1' fullWidth={fullView} fullHeight={fullView} />
                    <Chart title='RPS / Error rate' fullWidth fullHeight={fullView} />
                </div>
            }
        </div>);
}