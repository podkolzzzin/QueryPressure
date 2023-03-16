import React from 'react';
import {getInputType} from '@utils/GetInputType';

import {LimitProps} from './Limit.models';

export function Limit({limits, selectLimit, selectedLimit}: LimitProps) {
  return (
    <div className="card border-secondary mb-3">
      <div className="card-header">Limit</div>
      <div className="card-body text-dark">
        <div className="mb-3">
          <select className="form-select w-100" defaultValue="" title="Limit"
                  onChange={(event) => selectLimit(event.target.value)}
                  required>
            <option className="d-none" value="" disabled>Select Limit...</option>
            {
              limits.map((limit) => (<option key={limit.type} value={limit.type}>{limit.type}</option>))
            }
          </select>
        </div>
        {
          selectedLimit &&
          selectedLimit.arguments.map(arg =>
            (
              <div className="mb-3" key={'limit-argument-' + arg.name}>
                <label htmlFor={'limit-argument-' + arg.name} className="form-label">Limit - {arg.name}</label>
                <input type={getInputType(arg.type)} className="form-control" id={'limit-argument-' + arg.name}
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
