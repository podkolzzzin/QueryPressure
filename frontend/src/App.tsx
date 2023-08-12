import { initReactI18next } from 'react-i18next';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import i18n from 'i18next';

import { ExecutionPage } from './pages/execution/ExecutionPage';
import { IndexPage } from './pages/index/IndexPage';

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
