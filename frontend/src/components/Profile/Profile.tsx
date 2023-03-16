import React from 'react';
import {getInputType} from '@utils/GetInputType';

import {ProfileProps} from './Profile.models';

export function Profile({profiles, selectProfile, selectedProfile}: ProfileProps) {

  return (
    <div className="card border-secondary mb-3">
      <div className="card-header">Profile</div>
      <div className="card-body text-dark">
        <div className="mb-3">
          <select className="form-select w-100" defaultValue="" title="Profile"
                  onChange={(event) => selectProfile(event.target.value)}
                  required>
            <option className="d-none" value="" disabled>Select profile...</option>
            {
              profiles.map(({type}) => (<option key={type} value={type}>{type}</option>))
            }
          </select>
        </div>
        {
          selectedProfile &&
          selectedProfile.arguments.map(arg =>
            (
              <div className="mb-3" key={'profile-argument-' + arg.name}>
                <label htmlFor={'profile-argument-' + arg.name} className="form-label">Profile - {arg.name}</label>
                <input type={getInputType(arg.type)} className="form-control" id={'profile-argument-' + arg.name}
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
