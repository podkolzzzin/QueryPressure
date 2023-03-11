import './App.css';
import * as monaco from "monaco-editor";

import React, {useEffect, useState} from "react";

import StatusBar from "./StatusBar/StatusBar";
import Editor from "./Editor/Editor";
import Profile from "./Profile/Profile";
import Limit from "./Limit/Limit";
import ConnectionString from "./ConnectionString/ConnectionString";
import {ValidationMessage} from "./ConnectionString/ConnectionStringProps";
import {LimitModel} from "./models/LimitModel";
import {ProfileModel} from "./models/ProfileModel";


function App() {
  const [connectionString, setConnectionString] = useState<string>("");
  const [connectionStringValidationMessage, setConnectionStringValidationMessage] = useState<ValidationMessage>(null!);
  const [selectedProvider, setSelectedProvider] = useState<string>(null!);
  const [selectedProfile, setSelectedProfile] = useState<ProfileModel>(null!);
  const [selectedLimit, setSelectedLimit] = useState<LimitModel>(null!);
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

  function execute() {
    const options = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
          provider: selectedProvider,
          connectionString: connectionString,
          script: monaco.editor.getEditors()[0].getValue(),
          profile: selectedProfile,
          limit: selectedLimit
        }
      )
    };

    fetch('/api/execution', options)
      .then(r=> r.json());
  }

  function testConnectionString()
  {
    if (!selectedProvider){
      setConnectionStringValidationMessage({
        isGood: false,
        message: "Pls select provider first."
      })
      return;
    }

    if (!connectionString){
      setConnectionStringValidationMessage({
        isGood: false,
        message: "Connection string is empty."
      })
      return;
    }

    const options = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
          provider: selectedProvider,
          connectionString: connectionString
        }
      )
    };

    fetch('/api/connection/test', options)
      .then(r=> r.json())
      .then(response => {
        setConnectionStringValidationMessage({
          isGood: true,
          message: 'Ok: ' + response.serverVersion
        });
      })
      .catch(err => {
        setConnectionStringValidationMessage({
          isGood: false,
          message: err.message
        });
      })
  }

  function loadProviders(): void {
    fetch('/api/providers')
      .then(r=> r.json())
      .then(providers => {
        setProviders(providers);
      })
  }

  function loadProfiles(): void {
    fetch('/api/profiles')
      .then(r=> r.json())
      .then(profiles => {
        setProfiles(profiles);
      })
  }

  function loadLimits(): void {
    fetch('/api/limits')
      .then(r=> r.json())
      .then(limits => {
        setLimits(limits);
      })
  }

  useEffect(() => {
    loadProviders();
    loadProfiles();
    loadLimits();
  }, []);

  return (
    <div className="d-flex w-100 bg-black bg-opacity-10 vh-100">
      <div className="p-3 d-flex min-h-100 flex-column w-25 justify-content-between bg-white m-3" style={{borderRadius: 15 + 'px'}}>
        <div className="configuration-section">
          <h3>Configuration</h3>
          <div className="mb-3">
            <span>Provider: {selectedProvider === "" ? "Not selected." : selectedProvider}</span>
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

        <button type="button" className="btn btn-primary" onClick={execute}>Execute</button>
      </div>
      <div className="w-100 d-flex flex-column m-3 bg-white p-3" style={{borderRadius: 15 + 'px'}}>
        <h3>Code editor</h3>
        <Editor/>
        <StatusBar status="Ready."
                   providers={providers}
                   selectProvider={(provider) => setSelectedProvider(provider)}/>
      </div>
    </div>
  )
}

export default App;
