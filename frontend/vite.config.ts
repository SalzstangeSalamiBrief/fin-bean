import react from "@vitejs/plugin-react";
import { defineConfig } from "vite";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    tsconfigPaths: true,
  },
  build: {
    minify: "oxc",
    cssMinify: true,
    target: "esnext",
    rolldownOptions: {
      output: {
        comments: false,
        cleanDir: true,
        minify: true,
        minifyInternalExports: true,
      },
    },
  },
});
