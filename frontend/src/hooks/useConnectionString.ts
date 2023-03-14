import {ValidationMessage} from "@models";
import {ConnectionService} from "@services";
import {useState} from "react";

export function useConnectionString(selectedProvider: string | null) {
  const [connectionString, setConnectionString] = useState<string | null>(null);
  const [connectionStringValidationMessage, setConnectionStringValidationMessage] = useState<ValidationMessage | null>(null);

  function testConnectionString() {
    ConnectionService.test(selectedProvider, connectionString)
      .then(message => setConnectionStringValidationMessage(message));
  }

  return {
    connectionString,
    setConnectionString,
    connectionStringValidationMessage,
    testConnectionString
  };
}
