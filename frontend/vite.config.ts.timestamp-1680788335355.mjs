// vite.config.ts
import react from "file:///C:/Users/andriipodkolzin/source/repos/review/QueryPressure/frontend/node_modules/@vitejs/plugin-react/dist/index.mjs";
import * as path from "path";
import { defineConfig } from "file:///C:/Users/andriipodkolzin/source/repos/review/QueryPressure/frontend/node_modules/vite/dist/node/index.js";
var __vite_injected_original_dirname = "C:\\Users\\andriipodkolzin\\source\\repos\\review\\QueryPressure\\frontend";
var vite_config_default = defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      "/api": {
        target: "http://localhost:5073",
        changeOrigin: true,
        secure: false
      }
    },
    port: 5173
  },
  resolve: {
    alias: [
      { find: "@", replacement: path.resolve(__vite_injected_original_dirname, "src") },
      { find: "@components", replacement: path.resolve(__vite_injected_original_dirname, "src", "components") },
      { find: "@utils", replacement: path.resolve(__vite_injected_original_dirname, "src", "utils") },
      { find: "@assets", replacement: path.resolve(__vite_injected_original_dirname, "src", "assets") },
      { find: "@models", replacement: path.resolve(__vite_injected_original_dirname, "src", "models") },
      { find: "@api", replacement: path.resolve(__vite_injected_original_dirname, "src", "api") },
      { find: "@services", replacement: path.resolve(__vite_injected_original_dirname, "src", "services") },
      { find: "@containers", replacement: path.resolve(__vite_injected_original_dirname, "src", "containers") },
      { find: "@hooks", replacement: path.resolve(__vite_injected_original_dirname, "src", "hooks") },
      { find: "~bootstrap", replacement: path.resolve(__vite_injected_original_dirname, "node_modules/bootstrap") }
    ]
  }
});
export {
  vite_config_default as default
};
//# sourceMappingURL=data:application/json;base64,ewogICJ2ZXJzaW9uIjogMywKICAic291cmNlcyI6IFsidml0ZS5jb25maWcudHMiXSwKICAic291cmNlc0NvbnRlbnQiOiBbImNvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9kaXJuYW1lID0gXCJDOlxcXFxVc2Vyc1xcXFxhbmRyaWlwb2Rrb2x6aW5cXFxcc291cmNlXFxcXHJlcG9zXFxcXHJldmlld1xcXFxRdWVyeVByZXNzdXJlXFxcXGZyb250ZW5kXCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ZpbGVuYW1lID0gXCJDOlxcXFxVc2Vyc1xcXFxhbmRyaWlwb2Rrb2x6aW5cXFxcc291cmNlXFxcXHJlcG9zXFxcXHJldmlld1xcXFxRdWVyeVByZXNzdXJlXFxcXGZyb250ZW5kXFxcXHZpdGUuY29uZmlnLnRzXCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ltcG9ydF9tZXRhX3VybCA9IFwiZmlsZTovLy9DOi9Vc2Vycy9hbmRyaWlwb2Rrb2x6aW4vc291cmNlL3JlcG9zL3Jldmlldy9RdWVyeVByZXNzdXJlL2Zyb250ZW5kL3ZpdGUuY29uZmlnLnRzXCI7aW1wb3J0IHJlYWN0IGZyb20gJ0B2aXRlanMvcGx1Z2luLXJlYWN0JztcclxuaW1wb3J0ICogYXMgcGF0aCBmcm9tICdwYXRoJztcclxuaW1wb3J0IHtkZWZpbmVDb25maWd9IGZyb20gJ3ZpdGUnO1xyXG5cclxuLy8gaHR0cHM6Ly92aXRlanMuZGV2L2NvbmZpZy9cclxuZXhwb3J0IGRlZmF1bHQgZGVmaW5lQ29uZmlnKHtcclxuICBwbHVnaW5zOiBbcmVhY3QoKV0sXHJcbiAgc2VydmVyOiB7XHJcbiAgICBwcm94eToge1xyXG4gICAgICAnL2FwaSc6IHtcclxuICAgICAgICB0YXJnZXQ6ICdodHRwOi8vbG9jYWxob3N0OjUwNzMnLFxyXG4gICAgICAgIGNoYW5nZU9yaWdpbjogdHJ1ZSxcclxuICAgICAgICBzZWN1cmU6IGZhbHNlXHJcbiAgICAgIH0sXHJcbiAgICB9LFxyXG4gICAgcG9ydDogNTE3M1xyXG4gIH0sXHJcbiAgcmVzb2x2ZToge1xyXG4gICAgYWxpYXM6IFtcclxuICAgICAge2ZpbmQ6ICdAJywgcmVwbGFjZW1lbnQ6IHBhdGgucmVzb2x2ZShfX2Rpcm5hbWUsICdzcmMnKX0sXHJcbiAgICAgIHtmaW5kOiAnQGNvbXBvbmVudHMnLCByZXBsYWNlbWVudDogcGF0aC5yZXNvbHZlKF9fZGlybmFtZSwgJ3NyYycsICdjb21wb25lbnRzJyl9LFxyXG4gICAgICB7ZmluZDogJ0B1dGlscycsIHJlcGxhY2VtZW50OiBwYXRoLnJlc29sdmUoX19kaXJuYW1lLCAnc3JjJywgJ3V0aWxzJyl9LFxyXG4gICAgICB7ZmluZDogJ0Bhc3NldHMnLCByZXBsYWNlbWVudDogcGF0aC5yZXNvbHZlKF9fZGlybmFtZSwgJ3NyYycsICdhc3NldHMnKX0sXHJcbiAgICAgIHtmaW5kOiAnQG1vZGVscycsIHJlcGxhY2VtZW50OiBwYXRoLnJlc29sdmUoX19kaXJuYW1lLCAnc3JjJywgJ21vZGVscycpfSxcclxuICAgICAge2ZpbmQ6ICdAYXBpJywgcmVwbGFjZW1lbnQ6IHBhdGgucmVzb2x2ZShfX2Rpcm5hbWUsICdzcmMnLCAnYXBpJyl9LFxyXG4gICAgICB7ZmluZDogJ0BzZXJ2aWNlcycsIHJlcGxhY2VtZW50OiBwYXRoLnJlc29sdmUoX19kaXJuYW1lLCAnc3JjJywgJ3NlcnZpY2VzJyl9LFxyXG4gICAgICB7ZmluZDogJ0Bjb250YWluZXJzJywgcmVwbGFjZW1lbnQ6IHBhdGgucmVzb2x2ZShfX2Rpcm5hbWUsICdzcmMnLCAnY29udGFpbmVycycpfSxcclxuICAgICAge2ZpbmQ6ICdAaG9va3MnLCByZXBsYWNlbWVudDogcGF0aC5yZXNvbHZlKF9fZGlybmFtZSwgJ3NyYycsICdob29rcycpfSxcclxuICAgICAge2ZpbmQ6ICd+Ym9vdHN0cmFwJywgcmVwbGFjZW1lbnQ6IHBhdGgucmVzb2x2ZShfX2Rpcm5hbWUsICdub2RlX21vZHVsZXMvYm9vdHN0cmFwJyl9XHJcbiAgICBdXHJcbiAgfVxyXG59KTtcclxuIl0sCiAgIm1hcHBpbmdzIjogIjtBQUEyWSxPQUFPLFdBQVc7QUFDN1osWUFBWSxVQUFVO0FBQ3RCLFNBQVEsb0JBQW1CO0FBRjNCLElBQU0sbUNBQW1DO0FBS3pDLElBQU8sc0JBQVEsYUFBYTtBQUFBLEVBQzFCLFNBQVMsQ0FBQyxNQUFNLENBQUM7QUFBQSxFQUNqQixRQUFRO0FBQUEsSUFDTixPQUFPO0FBQUEsTUFDTCxRQUFRO0FBQUEsUUFDTixRQUFRO0FBQUEsUUFDUixjQUFjO0FBQUEsUUFDZCxRQUFRO0FBQUEsTUFDVjtBQUFBLElBQ0Y7QUFBQSxJQUNBLE1BQU07QUFBQSxFQUNSO0FBQUEsRUFDQSxTQUFTO0FBQUEsSUFDUCxPQUFPO0FBQUEsTUFDTCxFQUFDLE1BQU0sS0FBSyxhQUFrQixhQUFRLGtDQUFXLEtBQUssRUFBQztBQUFBLE1BQ3ZELEVBQUMsTUFBTSxlQUFlLGFBQWtCLGFBQVEsa0NBQVcsT0FBTyxZQUFZLEVBQUM7QUFBQSxNQUMvRSxFQUFDLE1BQU0sVUFBVSxhQUFrQixhQUFRLGtDQUFXLE9BQU8sT0FBTyxFQUFDO0FBQUEsTUFDckUsRUFBQyxNQUFNLFdBQVcsYUFBa0IsYUFBUSxrQ0FBVyxPQUFPLFFBQVEsRUFBQztBQUFBLE1BQ3ZFLEVBQUMsTUFBTSxXQUFXLGFBQWtCLGFBQVEsa0NBQVcsT0FBTyxRQUFRLEVBQUM7QUFBQSxNQUN2RSxFQUFDLE1BQU0sUUFBUSxhQUFrQixhQUFRLGtDQUFXLE9BQU8sS0FBSyxFQUFDO0FBQUEsTUFDakUsRUFBQyxNQUFNLGFBQWEsYUFBa0IsYUFBUSxrQ0FBVyxPQUFPLFVBQVUsRUFBQztBQUFBLE1BQzNFLEVBQUMsTUFBTSxlQUFlLGFBQWtCLGFBQVEsa0NBQVcsT0FBTyxZQUFZLEVBQUM7QUFBQSxNQUMvRSxFQUFDLE1BQU0sVUFBVSxhQUFrQixhQUFRLGtDQUFXLE9BQU8sT0FBTyxFQUFDO0FBQUEsTUFDckUsRUFBQyxNQUFNLGNBQWMsYUFBa0IsYUFBUSxrQ0FBVyx3QkFBd0IsRUFBQztBQUFBLElBQ3JGO0FBQUEsRUFDRjtBQUNGLENBQUM7IiwKICAibmFtZXMiOiBbXQp9Cg==
