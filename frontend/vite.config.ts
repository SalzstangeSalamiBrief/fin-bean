import { paraglideVitePlugin } from '@inlang/paraglide-js';
import tailwindcss from '@tailwindcss/vite';
import { devtools } from '@tanstack/devtools-vite';
import { tanstackStart } from '@tanstack/react-start/plugin/vite';
import { defineConfig } from 'vite';

import viteReact from '@vitejs/plugin-react';
import { nitro } from 'nitro/vite';
import { translatedPathnames } from './src/i18n/lib';

const config = defineConfig({
  resolve: { tsconfigPaths: true },
  plugins: [
    devtools(),
    paraglideVitePlugin({
      project: './paraglide.inlang',
      outdir: './src/paraglide.inlang',
      strategy: ['url', 'baseLocale'],
      urlPatterns: translatedPathnames,
      emitGitIgnore: true,
    }),
    nitro({ rollupConfig: { external: [/^@sentry\//] } }),
    tailwindcss(),
    tanstackStart(),
    viteReact(),
    tailwindcss(),
  ],
  build: {
    minify: 'oxc',
    cssMinify: true,
    target: 'esnext',
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

export default config;
