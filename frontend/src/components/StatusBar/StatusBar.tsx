import {StatusBarProps} from "./StatusBar.models";

function StatusBar(props: StatusBarProps) {

  return (
    <div className="status-bar px-2 row justify-content-between align-items-center">
      <span className="col-6 col-xl-9">{props.status}</span>
      <select className="form-select-sm col-6 col-xl-3" title="Provider"
              onChange={(event) => props.selectProvider(event.target.value)} value={props.selectedProvider ?? ""}>
        <option className="d-none" value="" disabled>Select provider...</option>
        {
          props.providers.map((provider) => (<option key={provider} value={provider}>{provider}</option>))
        }
      </select>
    </div>
  );
}

export default StatusBar;

