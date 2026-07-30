# fin-bean

A personal finance tracking app.

## Local development environment

The local dev environment (PostgreSQL + Keycloak) is provided via Docker Compose.

### Prerequisites

- Docker and Docker Compose

### Start the environment

```bash
cd docker
cp .env.template .env
docker compose up -d
```

This starts:

- **PostgreSQL** on `localhost:5432` (db/user/password from `docker/.env`, defaults: `finbean` / `finbean` / `changeme-local-dev`)
- **Keycloak** on `http://localhost:8080` (dev mode), with the `finbean` realm and `finbean-frontend` client auto-provisioned from `docker/keycloak/realm-export.json`
  - Admin console: `http://localhost:8080` - login with `KEYCLOAK_ADMIN` / `KEYCLOAK_ADMIN_PASSWORD` from `docker/.env` (defaults: `admin` / `changeme-local-dev`)
  - Register/login/logout for the app is done against the `finbean` realm; new users can self-register

Stop the environment with:

```bash
docker compose down
```

Add `-v` to also remove the Postgres volume (wipes all local data).

### Backend & frontend configuration

Copy the env templates and fill in values matching your `docker/.env`:

```bash
cp backend/.env.template backend/.env
cp frontend/.env.template frontend/.env
```

- `backend/.env.template` - PostgreSQL connection string + Keycloak authority/audience for the API
- `frontend/.env.template` - API base URL + Keycloak URL/realm/client for the SPA

Real `.env` files are git-ignored and must never be committed; only the `.env.template` files are tracked.
