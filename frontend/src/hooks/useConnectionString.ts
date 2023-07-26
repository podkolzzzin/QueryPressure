import {SetStateAction, useState} from 'react';
import {ValidationMessage} from '@models';
import {ConnectionService} from '@services';

import { ConnectionStringService } from '@/services/ConnectionStringService';

export function useConnectionString(selectedProvider: string | null) {
  const [connectionString, setConnectionString] = useState<string>(ConnectionStringService.load());
  const [connectionStringValidationMessage, setConnectionStringValidationMessage] = useState<ValidationMessage | null>(null);

  function testConnectionString() {
    ConnectionService.test(selectedProvider, connectionString)
      .then(message => setConnectionStringValidationMessage(message));
  }

  function setConnectionStringToUrl(connectionString: string) {
    ConnectionStringService.store(<string>connectionString);
    setConnectionString(connectionString);
  } 

  return {
    connectionString,
    setConnectionString: setConnectionStringToUrl,
    connectionStringValidationMessage,
    testConnectionString
  };
}
