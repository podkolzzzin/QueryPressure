import * as monaco from "monaco-editor";
import {useEffect, useRef} from "react";

function Editor() {
    function initEditor() {
        if (ref.current != null && monaco.editor.getEditors().length == 0) {
            monaco.editor.create((ref.current as HTMLElement), {
                value: 'SELECT * FROM [dbo].[AspNetUsers]',
                language: 'sql',
            });
        }
    }

    const ref = useRef(null);

    useEffect(() => {
        initEditor();
    });

    return (
      <div ref={ref} className="h-100" style={{minHeight: '400px'}}></div>
    );
}

export default Editor;
