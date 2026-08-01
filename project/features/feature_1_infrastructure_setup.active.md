# Infrastructure & environment setup

## Description

Provide a Docker-based local development environment for fin-bean. Uses docker compose to start PostgreSQL (data persistence) and Keycloak (authentication). Provides `.env.template` files with placeholder values for both frontend and backend; the real `.env` files are git-ignored and never pushed.

## Acceptance criteria

- `docker/` contains a docker compose setup that starts PostgreSQL and Keycloak.
- A Keycloak realm and client are provisioned for the app (register/login/logout capable).
- `.env.template` files exist for frontend and backend containing placeholder values only.
- Real `.env` files are listed in `.gitignore` and are not committed.
- `readme.md` documents how to start the local environment.

## implementation

- Added `docker/docker-compose.yml` with pinned images `postgres:17.2-alpine` and `quay.io/keycloak/keycloak:26.0.7`, env-var-driven credentials/ports (no hardcoded secrets), a named volume for Postgres data, a Postgres healthcheck, and ports bound to `127.0.0.1` only (loopback) to avoid exposing dev services on the local network.
- Added `docker/keycloak/realm-export.json`, auto-imported via `start-dev --import-realm`, provisioning the `finbean` realm with a public `finbean-frontend` client (PKCE S256, standard flow, self-registration enabled, redirect/web-origin scoped to `http://localhost:5173`).
- Added `docker/.env.template`, `backend/.env.template`, `frontend/.env.template` with placeholder-only values for DB connection info and Keycloak URL/realm/client settings.
- Updated `.gitignore` to keep real `.env` files ignored while explicitly un-ignoring `.env.template` files.
- Updated root `readme.md` with setup/teardown instructions, default ports, and Keycloak admin credentials location.
- Security review: found/fixed a medium-severity issue (ports were bound to `0.0.0.0`, now loopback-only). No other blocking issues.
- QA: all 5 acceptance criteria verified against a live `docker compose up` run (Postgres healthy, Keycloak realm/client imported and reachable via OIDC discovery, `.env` correctly git-ignored, readme steps accurate). No blocking bugs.
