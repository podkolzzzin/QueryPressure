import React from 'react';
import { useTranslation } from 'react-i18next';
import {getInputType} from '@utils/GetInputType';

import {LimitProps} from './Limit.models';

export function Limit({limits, selectLimit, selectedLimit}: LimitProps) {

  const { t } = useTranslation();

  return (
    <div className="card mb-3">
      <div className="card-header">{t('labels.limit')}</div>
      <div className="card-body">
        <div className="mb-3">
          <select className="form-select w-100" defaultValue="" title="Limit"
                  onChange={(event) => selectLimit(event.target.value)}
                  required>
            <option className="d-none" value="" disabled>{t('labels.selectLimit')}</option>
            {
              limits.map((limit) => (<option key={limit.type} value={limit.type}  title={t(`limits.${limit.type}.description`)!}>{t(`limits.${limit.type}.title`)}</option>))
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
