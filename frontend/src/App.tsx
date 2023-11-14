import { initReactI18next } from 'react-i18next';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import i18n from 'i18next';
import Backend from "i18next-http-backend";
import { HttpBackendOptions } from 'i18next-http-backend';
import { ResourcesApi } from '@/api/ResourcesApi';

import { ExecutionPage } from './pages/execution/ExecutionPage';
import { IndexPage } from './pages/index/IndexPage';

import './App.css';


// Initialize i18next with default options
i18n
  .use(initReactI18next)
  .use(Backend)
  .init<HttpBackendOptions>({
    fallbackLng: 'en-US',
    debug: true,
    interpolation: {
      escapeValue: false,
    },
    load: 'currentOnly',
    backend: {
      loadPath: '{{lng}}', // hack: get only localeId but not whole url in the request method
      request: (_options, localeId, _payload, callback) => {
        ResourcesApi.getAll(localeId)
          .then(data => callback(null, { status: 200, data }))
          .catch(error => callback(error, { status: 400, data: {} }));
      }
    }
  });

const router = createBrowserRouter([
  { path: '/', element: <IndexPage /> },
  { path: '/ui', element: <IndexPage /> },
  { path: '/execution/:id', element: <ExecutionPage /> },
]);
  

function App() {
  return (
    <div className="container-fluid px-0 px-xl-5">
      <RouterProvider router={router} />
    </div>
  );
}

export default App;
