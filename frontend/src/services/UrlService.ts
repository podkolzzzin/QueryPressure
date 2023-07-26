export const UrlService = {
    get(key: string) {
        return document.location.hash;
    }
};