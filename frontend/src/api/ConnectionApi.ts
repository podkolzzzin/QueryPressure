export interface TestConnectionRequestModel {
  provider: string;
  connectionString: string;
}

export interface TestConnectionStringResponse {
  serverVersion: string;
}

export const ConnectionApi = {
  test(model: TestConnectionRequestModel): Promise<TestConnectionStringResponse> {
    const options = {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify({
          provider: model.provider,
          connectionString: model.connectionString
        }
      )
    };

    return fetch('/api/connection/test', options)
      .then(r => r.json());
  }
};
