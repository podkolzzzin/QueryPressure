import React, {useState} from 'react';
import {ConfigurationCard, EditorCard} from '@containers';
import {useProvider} from '@hooks';

export function Layout() {
  const {providers, selectedProvider, selectProvider} = useProvider();
  const [script, setScript] = useState('');

  return (
    <div className="row justify-content-center gx-3 min-vh-100">
      <div className="col-xl-4 col-xxl-3 py-0 py-xl-5">
        <ConfigurationCard selectedProvider={selectedProvider} script={script}/>
      </div>
      <div className="col-xl-7 col-xxl-8 py-0 py-xl-5">
        <EditorCard providers={providers}
                    selectedProvider={selectedProvider}
                    selectProvider={selectProvider}
                    setScript={setScript}/>
      </div>
    </div>
  );
}
