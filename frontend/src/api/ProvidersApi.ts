export const ProvidersApi = {
  getAll(): Promise<string[]> {
    return fetch('/api/providers')
      .then(r => r.json());
  }
};
