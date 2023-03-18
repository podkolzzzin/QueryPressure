import React, {BaseSyntheticEvent} from 'react';
import {ExecutionApi} from '@api';
import {ConnectionString, Limit, Profile, ThemeSwitchButton} from '@components';
import {useConnectionString, useLimit, useProfile} from '@hooks';

import {ConfigurationCardProps} from './ConfigurationCardProps';

export function ConfigurationCard({selectedProvider, script, toggleTheme}: ConfigurationCardProps) {
  const {profiles, selectedProfile, selectProfile} = useProfile();
  const {limits, selectedLimit, selectLimit} = useLimit();
  const {
    connectionString,
    connectionStringValidationMessage,
    setConnectionString,
    testConnectionString,
  } = useConnectionString(selectedProvider);

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
              <h5 className="card-title">Configuration</h5>
              <ThemeSwitchButton toggleTheme={toggleTheme}/>
            </div>
            <div className="mb-3">
              <span>Provider: {selectedProvider ?? 'Not selected.'}</span>
            </div>

            <ConnectionString changed={setConnectionString}
                              test={testConnectionString}
                              validationMessage={connectionStringValidationMessage}/>

            <Profile profiles={profiles}
                     selectedProfile={selectedProfile}
                     selectProfile={selectProfile}/>

            <Limit limits={limits}
                   selectedLimit={selectedLimit}
                   selectLimit={selectLimit}/>
          </div>

          <button type="submit" className="btn btn-primary w-100">Execute</button>
        </form>
      </div>
    </div>
  );
}
