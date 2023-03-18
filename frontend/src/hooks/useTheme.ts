import {useEffect, useLayoutEffect, useState} from 'react';
import {bootstrapThemeAttribute, Theme, themeLocalStorageKey, themes} from '@models/theme';
import {LocalStorageService} from '@services';
import {getBrowserPreferTheme} from '@utils/GetBrowserPreferTheme';

export function useTheme() {
  const [theme, setTheme] = useState<Theme>(themes.light);

  useEffect(() => {
    const savedTheme = LocalStorageService.get<Theme>(themeLocalStorageKey);
    if (savedTheme) {
      setTheme(savedTheme);
    } else {
      const theme = getBrowserPreferTheme();
      setTheme(theme);
    }
  }, []);

  useLayoutEffect(() => {
    document.documentElement.setAttribute(bootstrapThemeAttribute, theme);
  }, [theme]);

  function toggleTheme() {
    let selectedTheme: Theme;
    if (theme === themes.light) {
      selectedTheme = themes.dark;
    } else {
      selectedTheme = themes.light;
    }

    setTheme(selectedTheme);
    LocalStorageService.set<Theme>(themeLocalStorageKey, selectedTheme);
  }

  return {
    theme,
    toggleTheme
  };
}
