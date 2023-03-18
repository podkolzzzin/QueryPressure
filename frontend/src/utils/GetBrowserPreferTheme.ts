import {Theme, themes} from '@models/theme';

export function getBrowserPreferTheme(): Theme {
  if (window.matchMedia) {
    if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
      return themes.dark;
    }
  }

  return themes.light;
}
