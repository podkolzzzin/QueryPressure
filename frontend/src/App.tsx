import { initReactI18next } from 'react-i18next';
import {Layout} from '@containers';
import i18n from 'i18next';

import './App.css';

export const locale = 'en-US';

// Initialize i18next with default options
i18n
  .use(initReactI18next)
  .init({
    fallbackLng: locale,
    debug: true,
    interpolation: {
      escapeValue: false,
    },
  });

function App() {
  return (
    <div className="container-fluid px-0 px-xl-5">
      <Layout/>
    </div>
  );
}

export default App;
