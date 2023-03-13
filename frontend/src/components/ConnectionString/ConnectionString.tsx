import {EyeIcon, EyeSlashIcon} from "@assets/Icons";
import React, {useState} from "react";

import {ConnectionStringProps} from "./ConnectionString.models";

function ConnectionString(props: ConnectionStringProps) {
  const [connectionStringShown, setConnectionStringState] = useState<boolean>(false);

  function getInputType(): "text" | "password" {
    return connectionStringShown ? "text" : "password";
  }

  function getValidationClass() {
    return props.validationMessage!.isGood ? 'valid-feedback text-truncate w-100' : 'invalid-feedback text-truncate w-100';
  }

  return (
    <div className="mb-3">
      <div className="input-group">
        <label htmlFor="connectionString" className="form-label">Connection string</label>
      </div>
      <div className="input-group is-invalid is-valid">
        <input type={getInputType()} className="form-control" id="connectionString"
               onChange={(event) => props.changed(event.target.value)}
               required/>
        <button className="btn btn-outline-secondary" type="button"
                onClick={() => props.test()}>
          Test
        </button>
        <button className="btn btn-outline-secondary" type="button" title="Show password"
                onClick={() => setConnectionStringState(!connectionStringShown)}>
          {!connectionStringShown && <EyeIcon/>}
          {connectionStringShown && <EyeSlashIcon/>}
        </button>
      </div>
      {props.validationMessage &&
          <div className={getValidationClass()} title={props.validationMessage.message}>
            {props.validationMessage.message}
          </div>
      }
    </div>
  );
}

export default ConnectionString;
