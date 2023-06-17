import React from 'react';
import { useTranslation } from 'react-i18next';
import {StatusBar} from '@components';
import {EditorCardProps} from '@containers/EditorCard/EditorCard.models';
import Editor from '@monaco-editor/react';

export function EditorCard({providers, selectedProvider, selectProvider, setScript, theme}: EditorCardProps) {
  const { t } = useTranslation();
  
  function handleEditorChange(value: string | undefined) {
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
