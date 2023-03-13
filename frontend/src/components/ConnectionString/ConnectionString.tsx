import {EyeIcon, EyeSlashIcon} from "@assets/Icons";
import React, {useState} from "react";

import {ConnectionStringProps} from "./ConnectionString.models";

export function ConnectionString({validationMessage, changed, test}: ConnectionStringProps) {
  const [connectionStringShown, setConnectionStringState] = useState<boolean>(false);
  const inputType = connectionStringShown ? "text" : "password";
  const validityString = validationMessage?.isGood ? "valid" : "invalid";

  return (
    <div className="mb-3">
      <div className="input-group">
        <label htmlFor="connectionString" className="form-label">Connection string</label>
      </div>
      <div className="input-group is-invalid is-valid">
        <input type={inputType} className="form-control" id="connectionString"
               onChange={(event) => changed(event.target.value)}
               required/>
        <button className="btn btn-outline-secondary" type="button"
                onClick={() => test()}>
          Test
        </button>
        <button className="btn btn-outline-secondary" type="button" title="Show password"
                onClick={() => setConnectionStringState(!connectionStringShown)}>
          {!connectionStringShown && <EyeIcon/>}
          {connectionStringShown && <EyeSlashIcon/>}
        </button>
      </div>
      {validationMessage &&
          <div className={`${validityString}-feedback text-truncate w-100`} title={validationMessage.message}>
            {validationMessage.message}
          </div>
      }
    </div>
  );
}
