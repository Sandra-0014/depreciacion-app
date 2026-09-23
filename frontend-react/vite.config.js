import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

export default defineConfig({
  plugins: [react()],
  server: {
    host: true,
    proxy: {
      '/api': {
        target: 'https://localhost:7129',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})