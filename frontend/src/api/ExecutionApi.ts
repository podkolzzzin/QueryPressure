import {LimitModel, ProfileModel} from '@models';

export interface ExecutionRequestModel {
  provider: string | null;
  connectionString: string | null;
  script: string | null;
  profile: ProfileModel | null;
  limit: LimitModel | null;
}

export const ExecutionApi = {
  run(model: Required<ExecutionRequestModel>): Promise<string> {
    const options = {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify(model)
    };

    return fetch('/api/execution', options)
      .then(r => r.json());
  },
  cancel(executionId: string) {
    const options = {
      method: 'POST',
    };

    return fetch(`/api/execution/${executionId}/cancel`, options);
  }
};
