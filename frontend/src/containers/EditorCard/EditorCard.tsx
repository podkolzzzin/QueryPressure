import React from 'react';
import {StatusBar} from '@components';
import Editor from '@monaco-editor/react';

export function EditorCard(
  {
    providers, selectedProvider, selectProvider, setScript
  }: { providers: string[], selectedProvider: string | null, selectProvider: (provider: string) => void, setScript: (script: string) => void }) {
  function handleEditorChange(value: string | undefined) {
    value && setScript(value);
  }

  return (
    <div className="card h-100">
      <div className="card-body d-flex flex-column">
        <h5 className="card-title">Code editor</h5>
        <div className="mb-2 h-100">
          {/*TODO: get default value based on selected provider (from backend)*/}
          <Editor
            defaultLanguage='sql'
            defaultValue='SELECT * FROM [dbo].[AspNetUsers]'
            options={{
              minimap: {enabled: false}
            }}
            onChange={handleEditorChange}
          />
        </div>
        <StatusBar status="Ready"
                   providers={providers}
                   selectedProvider={selectedProvider}
                   selectProvider={(provider) => selectProvider(provider)}/>
      </div>
    </div>
  );
}
