import {getInputType} from "@utils/GetInputType";
import React from "react";

import {ProfileProps} from "./Profile.models";

function Profile(props: ProfileProps) {

  return (
    <div className="card border-secondary mb-3">
      <div className="card-header">Profile</div>
      <div className="card-body text-dark">
        <div className="mb-3">
          <select className="form-select w-100" defaultValue="" title="Profile"
                  onChange={(event) => props.selectProfile(event.target.value)}
                  required>
            <option className="d-none" value="" disabled>Select profile...</option>
            {
              props.profiles.map(({type}) => (<option key={type} value={type}>{type}</option>))
            }
          </select>
        </div>
        {
          props.selectedProfile &&
          props.selectedProfile.arguments.map(arg =>
            (
              <div className="mb-3" key={"profile-argument-" + arg.name}>
                <label htmlFor={"profile-argument-" + arg.name} className="form-label">Profile - {arg.name}</label>
                <input type={getInputType(arg.type)} className="form-control" id={"profile-argument-" + arg.name}
                       onChange={(event) => arg.value = event.target.value}
                       required/>
              </div>
            )
          )
        }
      </div>
    </div>
  );
}

export default Profile;
