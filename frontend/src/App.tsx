import './App.css';

import {ExecutionApi} from "@api/ExecutionApi";
import {LimitsApi} from "@api/LimitsApi";
import {ProfilesApi} from "@api/ProfilesApi";
import {ProvidersApi} from "@api/ProvidersApi";
import {
  ConnectionString,
  Editor,
  Limit,
  Profile,
  StatusBar
} from "@components";
import {LimitModel, ProfileModel, ValidationMessage} from "@models";
import {ConnectionService} from "@services/ConnectionService";
import {EditorService} from "@services/EditorService";
import {ProviderService} from "@services/ProviderService";
import React, {BaseSyntheticEvent, useEffect, useState} from "react";


function App() {
  const [connectionString, setConnectionString] = useState<string | null>(null);
  const [connectionStringValidationMessage, setConnectionStringValidationMessage] = useState<ValidationMessage | null>(null);

  const [selectedProvider, setSelectedProvider] = useState<string | null>(null);
  const [selectedProfile, setSelectedProfile] = useState<ProfileModel | null>(null);
  const [selectedLimit, setSelectedLimit] = useState<LimitModel | null>(null);

  const [providers, setProviders] = useState<string[]>([]);
  const [profiles, setProfiles] = useState<ProfileModel[]>([]);
  const [limits, setLimits] = useState<LimitModel[]>([]);

  function selectProfile(profileType: string) {
    const profile: ProfileModel = profiles.find(p => p.type === profileType)!;
    setSelectedProfile(profile);
  }

  function selectLimit(limitType: string) {
    const limit: LimitModel = limits.find(p => p.type === limitType)!;
    setSelectedLimit(limit);
  }

  function selectProvider(provider: string) {
    setSelectedProvider(provider);
    ProviderService.saveCurrent(provider);
  }

  function execute(event: BaseSyntheticEvent) {
    event.preventDefault();

    ExecutionApi.run({
      provider: selectedProvider,
      connectionString: connectionString,
      script: EditorService.getValue(),
      profile: selectedProfile,
      limit: selectedLimit
    }).then(() => {
      /* TODO: processing result */
    });
  }

  function testConnectionString() {
    ConnectionService.test(selectedProvider, connectionString)
      .then(message => setConnectionStringValidationMessage(message));
  }

  function loadProviders(): void {
    ProvidersApi
      .getAll()
      .then(providers => setProviders(providers));

    const currentProvider = ProviderService.getCurrent();
    setSelectedProvider(currentProvider);
  }

  function loadProfiles(): void {
    ProfilesApi
      .getAll()
      .then(profiles => setProfiles(profiles));
  }

  function loadLimits(): void {
    LimitsApi
      .getAll()
      .then(limits => setLimits(limits));
  }

  useEffect(() => {
    loadProviders();
    loadProfiles();
    loadLimits();
  }, []);

  return (
    <div className="container-fluid px-0 px-xl-5">
      <div className="row justify-content-center gx-3 min-vh-100">
        <div className="col-xl-4 col-xxl-3 py-0 py-xl-5">
          <div className="card h-100">
            <div className="card-body">
              <form className="d-flex flex-column justify-content-between h-100" onSubmit={execute}>
                <div className="configuration-section">
                  <h5 className="card-title">Configuration</h5>
                  <div className="mb-3">
                    <span>Provider: {selectedProvider ?? "Not selected."}</span>
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
        </div>
        <div className="col-xl-7 col-xxl-8 py-0 py-xl-5">
          <div className="card h-100">
            <div className="card-body d-flex flex-column">
              <h5 className="card-title">Code editor</h5>
              <div className="mb-2 h-100">
                <Editor/>
              </div>
              <StatusBar status="Ready"
                         providers={providers}
                         selectedProvider={selectedProvider}
                         selectProvider={(provider) => selectProvider(provider)}/>
            </div>
          </div>

        </div>
      </div>
    </div>
  );
}

export default App;
