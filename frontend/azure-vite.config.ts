import {defineConfig} from 'vite';

import {baseConfig} from './vite.config';

export default defineConfig({
  ...baseConfig,
  server: {
    proxy: {
      '/api': {
        target: 'https://querypressure-dev.azurewebsites.net',
        changeOrigin: true,
        secure: true
      },
      '/ws': {
        target: 'ws://querypressure-dev.azurewebsites.net',
        changeOrigin: true,
        secure: true
      }
    }
  },
});
