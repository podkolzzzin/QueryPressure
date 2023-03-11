import {LimitProps} from "./LimitProps";
import React from "react";
import {getInputType} from "../helpers/GetInputType";

function Limit(props: LimitProps) {
  return (
    <div className="card border-dark mb-3">
      <div className="card-header">Limit</div>
      <div className="card-body text-dark">
        <div className="mb-3">
          <select className="form-select w-100" defaultValue="" title="Limit"
                  onChange={(event) => props.selectLimit(event.target.value)}>
            <option className="d-none" value="" disabled>Select Limit...</option>
            {
              props.limits.map((limit) => (<option key={limit.type} value={limit.type}>{limit.type}</option>))
            }
          </select>
        </div>
        {
          props.selectedLimit &&
          props.selectedLimit.arguments.map(arg =>
            (
              <div className="mb-3" key={"limit-argument-" + arg.name}>
                <label htmlFor={"limit-argument-" + arg.name} className="form-label">Limit - {arg.name}</label>
                <input type={getInputType(arg.type)} className="form-control" id={"limit-argument-" + arg.name}
                       onChange={(event) => arg.value = event.target.value}/>
              </div>
            )
          )
        }
      </div>
    </div>
  );
}

export default Limit;
