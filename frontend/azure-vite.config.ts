import react from '@vitejs/plugin-react';
import * as path from 'path';
import {defineConfig} from 'vite';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
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
    },
    port: 5173
  },
  resolve: {
    alias: [
      {find: '@', replacement: path.resolve(__dirname, 'src')},
      {find: '@components', replacement: path.resolve(__dirname, 'src', 'components')},
      {find: '@utils', replacement: path.resolve(__dirname, 'src', 'utils')},
      {find: '@assets', replacement: path.resolve(__dirname, 'src', 'assets')},
      {find: '@models', replacement: path.resolve(__dirname, 'src', 'models')},
      {find: '@api', replacement: path.resolve(__dirname, 'src', 'api')},
      {find: '@services', replacement: path.resolve(__dirname, 'src', 'services')},
      {find: '@containers', replacement: path.resolve(__dirname, 'src', 'containers')},
      {find: '@hooks', replacement: path.resolve(__dirname, 'src', 'hooks')},
      {find: '~bootstrap', replacement: path.resolve(__dirname, 'node_modules/bootstrap')}
    ]
  }
});
