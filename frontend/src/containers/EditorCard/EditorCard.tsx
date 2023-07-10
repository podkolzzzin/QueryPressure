import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import {StatusBar} from '@components';
import {EditorCardProps} from '@containers/EditorCard/EditorCard.models';
import Editor from '@monaco-editor/react';
import { FileUpload } from '@/components/FileUploader/FileUploader';

export function EditorCard({providers, selectedProvider, selectProvider, setScript, theme}: EditorCardProps) {
  const { t } = useTranslation();
  const [editorContent, setEditorContent] = useState('');
  const supportedFileTypes = [ 'txt', 'sql', 'js', 'lua', 'json' ]
  
  function handleEditorChange(value: string | undefined) {
    value && setEditorContent(value);
    value && setScript(value);
  }

  function handleEditorMount(editor: any) {
    const value = `
    SELECT
    t.relname AS table_name,
    a.attname AS column_name,
    CASE
      WHEN a.attnotnull THEN 'NOT NULL'
      ELSE ''
    END AS constraints,
    i.relname AS index_name
  FROM
    pg_class t
  JOIN
    pg_attribute a ON t.oid = a.attrelid
  LEFT JOIN
    pg_index ix ON t.oid = ix.indrelid AND a.attnum = ANY(ix.indkey)
  LEFT JOIN
    pg_class i ON ix.indexrelid = i.oid
  WHERE
    t.relkind = 'r'  -- Only regular tables
    AND t.relname NOT LIKE 'pg_%'  -- Exclude system tables
    AND t.relname NOT LIKE 'sql_%'  -- Exclude SQL-standard tables
  ORDER BY
    t.relname, a.attnum;`;
    editor.setValue(value);
    handleEditorChange(value);
  }

  function handleDragOver(event: any) {
    event.preventDefault();
  };
  
  function handleDrop(event: any) {
    event.preventDefault();
    
    const file = event.dataTransfer.files[0];
    const fileType = file.name.split('.').pop();

    if(fileType && supportedFileTypes.includes(fileType)) {
      const reader = new FileReader();
  
      reader.onload = (e : any) => {
        const fileContent = e.target.result;
        // Update the editor's content with the file content
        handleEditorChange(fileContent);
      };
    
      reader.readAsText(file); 
    }
  };

  function handleFileUpload(file : any) {
    const reader = new FileReader();

    reader.onload = (e : any) => {
      const fileContent = e.target.result;
      handleEditorChange(fileContent);
    };

    reader.readAsText(file);
  };
  

  return (
    <div className="card h-100">
      <div className="card-body d-flex flex-column">
        <h5 className="card-title">{t('labels.codeEditor')}</h5>
        <div className="mb-2 h-100" onDragOver={handleDragOver} onDrop={handleDrop}>
          {/*TODO: get default value based on selected provider (from backend)*/}
          <Editor
              value={editorContent}
              defaultLanguage='sql'
              options={{
                minimap: { enabled: false }
              }}
              onChange={handleEditorChange}
              onMount={handleEditorMount}
              defaultValue=''
              theme={theme === 'dark' ? 'vs-dark' : 'light'}/>
        </div>

        <StatusBar status="Ready"
                   providers={providers}
                   selectedProvider={selectedProvider}
                   selectProvider={(provider) => selectProvider(provider)}
                   handleFileUpload={handleFileUpload}
                   allowedFileTypes={supportedFileTypes}/>
      </div>
    </div>
  );
}
