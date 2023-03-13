import * as monaco from "monaco-editor";
import React from "react";

export const EditorService = {
  init(ref: React.RefObject<HTMLDivElement>): void {
    if (ref.current != null && monaco.editor.getEditors().length == 0) {
      monaco.editor.create((ref.current as HTMLElement), {
        // TODO: get default value based on selected provider (from backend)
        value: 'SELECT * FROM [dbo].[AspNetUsers]',
        language: 'sql',
        minimap: {enabled: false}
      });
    }
  },
  getValue(): string {
    return monaco.editor.getEditors()[0].getValue();
  }
};
