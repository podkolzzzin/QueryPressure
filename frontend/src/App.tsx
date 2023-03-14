import './App.css';

import {ExecutionApi} from "@api";
import {ConnectionString, Editor, Limit, Profile, StatusBar} from "@components";
import {EditorService} from "@services";
import React, {BaseSyntheticEvent} from "react";

import {useConnectionString, useLimit, useProfile, useProvider} from "@/hooks";

function App() {
  const {profiles, selectedProfile, selectProfile} = useProfile();
  const {limits, selectedLimit, selectLimit} = useLimit();
  const {providers, selectedProvider, selectProvider} = useProvider();
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
      script: EditorService.getValue(),
      profile: selectedProfile,
      limit: selectedLimit
    }).then(() => {
      /* TODO: processing result */
    });
  }

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
