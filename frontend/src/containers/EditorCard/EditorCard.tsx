import React from 'react';
import {StatusBar} from '@components';
import {EditorCardProps} from '@containers/EditorCard/EditorCard.models';
import Editor from '@monaco-editor/react';
import { t } from 'i18next';

export function EditorCard({providers, selectedProvider, selectProvider, setScript, theme}: EditorCardProps) {
  function handleEditorChange(value: string | undefined) {
    value && setScript(value);
  }

  function handleEditorMount(editor: any) {
    const value = 'SELECT * FROM [dbo].[AspNetUsers]';
    editor.setValue(value);
    handleEditorChange(value);
  }

  return (
    <div className="card h-100">
      <div className="card-body d-flex flex-column">
        <h5 className="card-title">{t('labels.codeEditor')}</h5>
        <div className="mb-2 h-100">
          {/*TODO: get default value based on selected provider (from backend)*/}
          <Editor
            defaultLanguage='sql'
            defaultValue=''
            options={{
              minimap: {enabled: false}
            }}
            onChange={handleEditorChange}
            onMount={handleEditorMount}
            theme={theme === 'dark' ? 'vs-dark' : 'light'}
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
