import React from 'react';
import {MoonIcon, SunIcon} from '@assets/Icons';

import './theme-switch.css';

export function ThemeSwitchButton({toggleTheme}: { toggleTheme: () => void }) {
  return (
    <button className="theme-switch" type="button" role="switch" onClick={() => toggleTheme()}>
      <span className="theme-switch-check">
        <span className="theme-switch-icon">
          <SunIcon/>
          <MoonIcon/>
        </span>
      </span>
    </button>
  );
}
