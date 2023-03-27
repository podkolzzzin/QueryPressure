import React from 'react';
import { useTranslation } from 'react-i18next';
import {getInputType} from '@utils/GetInputType';

import {ProfileProps} from './Profile.models';

export function Profile({profiles, selectProfile, selectedProfile}: ProfileProps) {
  
  const { t } = useTranslation();
  
  return (
    <div className="card mb-3">
      <div className="card-header">{t('labels.profile')}</div>
      <div className="card-body">
        <div className="mb-3">
          <select className="form-select w-100" defaultValue="" title="Profile"
                  onChange={(event) => selectProfile(event.target.value)}
                  required>
            <option className="d-none" value="" disabled>{t('labels.selectProfile')}</option>
            {
              profiles.map(({type}) => (<option key={type} value={type} title={t(`profiles.${type}.description`)!}>{t(`profiles.${type}.title`)}</option>))
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
