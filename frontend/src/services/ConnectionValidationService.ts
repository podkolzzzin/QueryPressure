import {ValidationMessage} from '@models';

export const ConnectionValidationService = {
  test(selectedProvider: string | null, connectionString: string | null): ValidationMessage | null {
    if (!selectedProvider) {
      return {
        isGood: false,
        message: 'Provider not selected'
      };
    }

    if (!connectionString) {
      return {
        isGood: false,
        message: 'Connection string is empty'
      };
    }

    return null;
  }
};
