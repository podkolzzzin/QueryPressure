function updateUrl(url: URL): void {
    window.history.replaceState({}, '', url.toString());
}

export const UrlService = {
    get: (key: string): string | null => {
        const url = new URL(window.location.href);
        return url.searchParams.get(key);
    },

    set: (key: string, value: string): void => {
        const url = new URL(window.location.href);
        url.searchParams.set(key, value);
        updateUrl(url);
    },

    unset: (key: string): void => {
        const url = new URL(window.location.href);
        url.searchParams.delete(key);
        updateUrl(url);
    }
};
