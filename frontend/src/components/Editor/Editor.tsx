import {EditorService} from "@services/EditorService";
import {useEffect, useRef} from "react";

function Editor() {
    const ref = useRef<HTMLDivElement>(null);

    useEffect(() => {
        EditorService.init(ref);
    }, []);

    return (
      <div ref={ref} className="h-100" style={{minHeight: '400px'}}></div>
    );
}

export default Editor;
