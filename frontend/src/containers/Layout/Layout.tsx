import React, {useState} from 'react';
import {ConfigurationCard, EditorCard} from '@containers';
import {useProvider, useToggle} from '@hooks';
import {useTheme} from '@hooks/useTheme';

export function Layout() {
  const {providers, selectedProvider, selectProvider} = useProvider();
  const [script, setScript] = useState('');
  const [executionId, setExecutionId] = useState('');
  const {theme, toggleTheme} = useTheme();
  const [showMonitor, setShowMonitor] = useState(false);
  const [ cancelButtonEnabled, toggleCancelButton ] = useToggle();

  const openMonitoring = () => setShowMonitor(true);
  const toggleMonitor = () => setShowMonitor(prev => !prev);

  return (
    <div className="row justify-content-center gx-3 min-vh-100">
      <div className="col-xl-4 col-xxl-3 py-0 py-xl-5">
        <ConfigurationCard  selectedProvider={selectedProvider} 
                            script={script} 
                            executionId={executionId}
                            cancelButtonEnabled={cancelButtonEnabled}
                            toggleTheme={toggleTheme} 
                            openMonitoring={openMonitoring}
                            setExecutionId={setExecutionId}
                            />
      </div>
      <div className="col-xl-7 col-xxl-8 py-0 py-xl-5">
        <EditorCard providers={providers}
                    executionId={executionId}
                    selectedProvider={selectedProvider}
                    selectProvider={selectProvider}
                    setScript={setScript}
                    theme={theme}
                    showMonitor={showMonitor}
                    toggleMonitor={toggleMonitor}
                    toggleCancelButton={toggleCancelButton}/>
      </div>
    </div>
  );
}
