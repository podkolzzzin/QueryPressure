import {EditorService} from "@services/EditorService";
import {memo, useEffect, useRef} from "react";

export const MonacoEditor = memo(function Editor() {
    const ref = useRef<HTMLDivElement>(null);

    useEffect(() => {
        EditorService.init(ref);
    }, []);

    return (
      <div ref={ref} className="h-100" style={{minHeight: '400px'}}></div>
    );
});
