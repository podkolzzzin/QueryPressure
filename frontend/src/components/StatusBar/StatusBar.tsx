import { useTranslation } from 'react-i18next';

import {StatusBarProps} from './StatusBar.models';
import { FileUpload } from '../FileUploader/FileUploader';

import './status-bar.css'

export function StatusBar({status, selectProvider, providers, selectedProvider, handleFileUpload, allowedFileTypes}: StatusBarProps) {
  const { t } = useTranslation();
  return (
    <div className="row status-bar px-2 align-items-center">
      <div className="col-sm-6 mt-1">
        <span>{status}</span>
      </div>

      <div className="col-sm-4 mt-1">
        <FileUpload onFileUpload={handleFileUpload} allowedFileTypes={allowedFileTypes} />
      </div>

      <div className="col-sm-2 mt-1">
        <select className="form-select form-select-sm select-provider" title="Provider"
          onChange={(event) => selectProvider(event.target.value)} value={selectedProvider ?? ''}>
          <option className="d-none" value="" disabled>{t('labels.selectProvider')}</option>
          {
            providers.map((provider) => (<option key={provider} value={provider}>{provider}</option>))
          }
        </select>
      </div>
    </div>
  );
}
