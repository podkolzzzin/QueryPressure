import {ConnectionApi} from '@api/ConnectionApi';
import {ValidationMessage} from '@models';
import {ConnectionValidationService} from '@services/ConnectionValidationService';

export const ConnectionService = {
  test(selectedProvider: string | null, connectionString: string | null): Promise<ValidationMessage> {
    const validationResult = ConnectionValidationService.test(selectedProvider, connectionString);
    if (validationResult){
      return Promise.resolve(validationResult);
    }

    return ConnectionApi
      .test({provider: selectedProvider!, connectionString: connectionString!})
      .then(response => {
        return {
          isGood: true,
          message: 'Ok: ' + response.serverVersion
        };
      })
      .catch(err => {
        return {
          isGood: false,
          message: err.message
        };
      });
  }
};
