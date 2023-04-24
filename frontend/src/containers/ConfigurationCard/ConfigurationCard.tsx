import React, { BaseSyntheticEvent, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { ExecutionApi } from '@api';
import { ConnectionString, Limit, Profile, ThemeSwitchButton } from '@components';
import { useConnectionString, useLimit, useProfile, useResources } from '@hooks';

import { ConfigurationCardProps } from './ConfigurationCardProps';

export function ConfigurationCard({ selectedProvider, script, toggleTheme }: ConfigurationCardProps) {
  const { profiles, selectedProfile, selectProfile } = useProfile();
  const { limits, selectedLimit, selectLimit } = useLimit();
  const {
    connectionString,
    connectionStringValidationMessage,
    setConnectionString,
    testConnectionString,
  } = useConnectionString(selectedProvider);
  const { t } = useTranslation();
  const { languages, language, changeLanguage } = useResources();

  function execute(event: BaseSyntheticEvent) {
    event.preventDefault();

    ExecutionApi.run({
      provider: selectedProvider,
      connectionString: connectionString,
      script: script,
      profile: selectedProfile,
      limit: selectedLimit
    }).then(() => {
      /* TODO: processing result */
    });
  }

  return (
    <div className="card h-100">
      <div className="card-body">
        <form className="d-flex flex-column justify-content-between h-100" onSubmit={execute}>
          <div className="configuration-section">
            <div className="d-flex justify-content-between">
              <h5 className="card-title">{t('labels.configuration')}</h5>
              <ThemeSwitchButton toggleTheme={toggleTheme} />
            </div>
            <div className="mb-3 d-flex justify-content-between">
              <span>{t('labels.provider')}: {selectedProvider ?? 'Not selected.'}</span>
              <select className="form-select form-select-sm w-auto" title="Provider"
                onChange={(event) => changeLanguage(event.target.value)} value={language}>
                <option className="d-none" value={language}>{language}</option>
                {languages.map(lngName => <option value={lngName} key={lngName}>{lngName}</option>)}
              </select>
            </div>
            <ConnectionString changed={setConnectionString}
              test={testConnectionString}
              validationMessage={connectionStringValidationMessage} />

            <Profile profiles={profiles}
              selectedProfile={selectedProfile}
              selectProfile={selectProfile} />

            <Limit limits={limits}
              selectedLimit={selectedLimit}
              selectLimit={selectLimit} />
          </div>

          <button type="submit" className="btn btn-primary w-100">{t('labels.execute')}</button>
        </form>
      </div>
    </div>
  );
}
