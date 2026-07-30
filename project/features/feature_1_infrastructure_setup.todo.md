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
