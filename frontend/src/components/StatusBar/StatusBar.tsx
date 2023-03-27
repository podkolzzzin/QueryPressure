import { useTranslation } from 'react-i18next';

import {StatusBarProps} from './StatusBar.models';

export function StatusBar({status, selectProvider, providers, selectedProvider}: StatusBarProps) {
  const { t } = useTranslation();
  return (
    <div className="status-bar px-2 row justify-content-between align-items-center">
      <span className="col-6 col-xl-9">{status}</span>
      <select className="form-select form-select-sm col-6 col-xl-3 w-auto" title="Provider"
              onChange={(event) => selectProvider(event.target.value)} value={selectedProvider ?? ''}>
        <option className="d-none" value="" disabled>{t('labels.selectProvider')}</option>
        {
          providers.map((provider) => (<option key={provider} value={provider}>{provider}</option>))
        }
      </select>
    </div>
  );
}
