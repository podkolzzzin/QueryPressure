export type Theme = 'light' | 'dark';
export const themes: { light: Theme, dark: Theme } = {
  light: 'light',
  dark: 'dark'
};

export const themeLocalStorageKey = 'theme';
export const bootstrapThemeAttribute = 'data-bs-theme';
