import react from '@vitejs/plugin-react';
import * as path from 'path';
import {defineConfig} from 'vite';

export const baseConfig = {
  plugins: [react()],
  server: {
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
  },
};

// https://vitejs.dev/config/
export default defineConfig({
  ...baseConfig,
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5073',
        changeOrigin: true,
        secure: false
      },
      '/ws': {
        target: 'ws://localhost:5073',
        changeOrigin: true,
        secure: false
      },
    }
  }
});
