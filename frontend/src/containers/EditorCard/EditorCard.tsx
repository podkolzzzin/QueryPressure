import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import {StatusBar} from '@components';
import {EditorCardProps} from '@containers/EditorCard/EditorCard.models';
import Editor, { Monaco } from '@monaco-editor/react';
import * as monaco from 'monaco-editor/esm/vs/editor/editor.api';
import { MonitoringScreen } from '@/components/MonitoringScreen';

export function EditorCard(
  {providers, executionId, selectedProvider, selectedProviderInfo, selectProvider, setScript, theme, showMonitor, toggleMonitor, toggleCancelButton}: EditorCardProps) {
  const { t } = useTranslation();
  const [editor, setEditor] = useState<monaco.editor.IStandaloneCodeEditor | null>(null);
  const [editorContent, setEditorContent] = useState<string | undefined>(undefined);
  const [editorLanguage, setEditorLanguage] = useState<string>('');
  const [registeredLanguages, setRegisteredLanguages] = useState<monaco.languages.ILanguageExtensionPoint[]>([]);
  const supportedFileTypes = [ 'txt', 'sql', 'js', 'lua', 'json' ];

  useEffect(() => {
    if (!selectedProviderInfo || !editor) return;

    const value = editor.getValue() || selectedProviderInfo?.initialScript || '';    
    const language = getScriptLanguageId(selectedProviderInfo?.syntaxAliases, 'sql');

    editor.setValue(value);
    setEditorLanguage(language);

    handleEditorChange(value);
  }, [selectedProviderInfo, editor]);
  
  function handleEditorChange(value: string | undefined) {
    value && setEditorContent(value);
    value && setScript(value);
  }

  function handleEditorMount(editor: monaco.editor.IStandaloneCodeEditor, e: Monaco) {
    setEditor(editor);
    setRegisteredLanguages(e.languages.getLanguages())
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

  function getScriptLanguageId(syntaxAliases: string[] | null, fallbackLanguage: string): string {
    if (!syntaxAliases) return fallbackLanguage;

    for (const language of syntaxAliases) {
      for (const lang of registeredLanguages) {
        if (lang.id === language || lang.aliases?.map(x => x.toLocaleLowerCase())?.includes(language.toLocaleLowerCase()))
          return lang.id;
      }
    }

    return fallbackLanguage;
  }

  return (
    <div className="card h-100 position-relative">
      <div className="card-body d-flex flex-column">
        <h5 className="card-title">{t('labels.codeEditor')}</h5>
        <div className="mb-2 h-100" onDragOver={handleDragOver} onDrop={handleDrop}>
          <Editor
              value={editorContent}
              defaultLanguage='sql'
              language={editorLanguage}
              options={{
                minimap: { enabled: false }
              }}
              onChange={handleEditorChange}
              onMount={handleEditorMount}
              defaultValue=''
              theme={theme === 'dark' ? 'vs-dark' : 'light'}/>
        </div>
        <MonitoringScreen executionId={executionId} showMonitor={showMonitor} toggleMonitor={toggleMonitor} toggleCancelButton={toggleCancelButton}/>
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
