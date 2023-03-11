import {StatusBarProps} from "./StatusBarProps";

function StatusBar(props: StatusBarProps) {

  return (
    <div className="status-bar px-2 d-flex justify-content-between align-items-center">
      <span>{props.status}</span>
      <select className="form-select-sm w-25" defaultValue="" title="Provider"
              onChange={(event) => props.selectProvider(event.target.value)}>
        <option className="d-none" value="" disabled>Select provider...</option>
        {
          props.providers.map((provider) => (<option key={provider} value={provider}>{provider}</option>))
        }
      </select>
    </div>
  )
}

export default StatusBar;

