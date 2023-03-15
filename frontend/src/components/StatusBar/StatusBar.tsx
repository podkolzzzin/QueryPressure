import {StatusBarProps} from './StatusBar.models';

export function StatusBar({status, selectProvider, providers, selectedProvider}: StatusBarProps) {

  return (
    <div className="status-bar px-2 row justify-content-between align-items-center">
      <span className="col-6 col-xl-9">{status}</span>
      <select className="form-select-sm col-6 col-xl-3" title="Provider"
              onChange={(event) => selectProvider(event.target.value)} value={selectedProvider ?? ''}>
        <option className="d-none" value="" disabled>Select provider...</option>
        {
          providers.map((provider) => (<option key={provider} value={provider}>{provider}</option>))
        }
      </select>
    </div>
  );
}
