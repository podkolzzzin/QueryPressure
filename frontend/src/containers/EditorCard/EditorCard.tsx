import {MonacoEditor, StatusBar} from "@components";
import React from "react";

export function EditorCard(
  {
    providers, selectedProvider, selectProvider
  }: { providers: string[], selectedProvider: string | null, selectProvider: (provider: string) => void }) {
  return (
    <div className="card h-100">
      <div className="card-body d-flex flex-column">
        <h5 className="card-title">Code editor</h5>
        <div className="mb-2 h-100">
          <MonacoEditor/>
        </div>
        <StatusBar status="Ready"
                   providers={providers}
                   selectedProvider={selectedProvider}
                   selectProvider={(provider) => selectProvider(provider)}/>
      </div>
    </div>
  );
}
