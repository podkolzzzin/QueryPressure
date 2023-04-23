export const LocalesApi = {
    get(): Promise<string[]> {
        return fetch(`/api/locales/`)
            .then(r => r.json());
    }
};
