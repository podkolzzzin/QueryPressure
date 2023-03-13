import {LocalStorageService} from "@services/LocalStorageService";

const currentProviderKey = 'currentProvider';
export const ProviderService = {
  saveCurrent(provider: string): void {
    LocalStorageService.set(currentProviderKey, provider);
  },
  getCurrent(): string | null {
    return LocalStorageService.get<string>(currentProviderKey);
  }
};
