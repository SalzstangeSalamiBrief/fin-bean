# Frontend foundation

## Description

The root folder for the frontend is /frontend. This folder already contains a basic scaffold that should be used as the starting point and built upon (do not recreate it from scratch).

Scaffold the React 19 SPA using Vite, TanStack Router, Tailwind CSS, Radix UI, Zustand and React Query. Generate the API client with Orval from the backend OpenAPI spec into `apis/`. Integrate Keycloak login/logout and attach the access token to authenticated API calls. Dark theme only (no light mode / toggle).

## Acceptance criteria

- SPA builds and runs with pnpm (npm must not be used).
- TanStack Router is configured with a `routes/` folder.
- Keycloak auth flow works (login, logout, token attached to API requests).
- Orval generates the API client from the backend OpenAPI spec into `apis/`.
- A global dark theme is applied.
- Unauthenticated users are redirected to login on protected routes.

## implementation
