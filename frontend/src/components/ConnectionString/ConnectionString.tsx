import React, {useState} from 'react';
import { useTranslation } from 'react-i18next';
import {EyeIcon, EyeSlashIcon} from '@assets/Icons';

import {ConnectionStringProps} from './ConnectionString.models';

export function ConnectionString({validationMessage, initialValue, changed, test}: ConnectionStringProps) {
  const [connectionStringShown, setConnectionStringState] = useState<boolean>(false);
  const { t } = useTranslation();
  const inputType = connectionStringShown ? 'text' : 'password';
  const validityString = validationMessage?.isGood ? 'valid' : 'invalid';

  return (
    <div className="mb-3">
      <div className="input-group">
        <label htmlFor="connectionString" className="form-label">{ t('labels.connectionString') }</label>
      </div>
      <div className="input-group is-invalid is-valid">
        <input type={inputType} className="form-control" id="connectionString"
               value={initialValue}
               onChange={(event) => changed(event.target.value)}
               required/>
        <button className="btn btn-outline-secondary" type="button"
                onClick={() => test()}>
          Test
        </button>
        <button className="btn btn-outline-secondary" type="button" title={t('labels.showConnectionString')!}
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
