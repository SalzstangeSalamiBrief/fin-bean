# Backend foundation & authenticated API

## Description

The root folder for the backend is /backend. This folder already contains a basic scaffold/solution that should be used as the starting point and built upon (do not recreate it from scratch).

Scaffold the .NET 10 backend using a Domain-Driven Design + multi-layer architecture (API, Core, Database projects). Use EF Core against PostgreSQL. Validate Keycloak-issued JWTs and resolve the current user from the token. Enforce that a user can only access their own data. Errors are returned using the ProblemDetails standard, and an OpenAPI document is exposed for Orval generation.

## Acceptance criteria

- Solution builds with API, Core and Database projects wired together.
- EF Core is connected to PostgreSQL and migrations can be created/applied.
- JWT bearer authentication validates tokens issued by Keycloak.
- Protected endpoints require authentication; user identity is resolved from the token.
- All data access is scoped to the authenticated user (no cross-user access).
- Error responses follow the ProblemDetails standard.
- An OpenAPI/Swagger document is exposed for the frontend to consume.

## implementation

- `FinBean.Database`: added `FinBeanDbContext` (Npgsql provider, no `DbSet`s yet — no domain data exists in this slice) and an `IDesignTimeDbContextFactory` so `dotnet ef` tooling works without booting the API host; added an initial (empty) `InitialCreate` migration proving the migration pipeline works end-to-end.
- `FinBean.Api`: wired JWT bearer authentication against the local Keycloak realm `finbean` (authority `http://localhost:8080/realms/finbean`, matching client `finbean-frontend`); `ValidateIssuer`/`ValidateAudience`/`ValidateLifetime` all enabled; `RequireHttpsMetadata` relaxed only in Development. Added `AddProblemDetails()` + exception/status-code-page middleware (no custom stack-trace leakage, only Development surfaces exception detail). `UseAuthentication()` added before `UseAuthorization()` (previously missing). `WeatherForecastController` now has `[Authorize]` and injects `ICurrentUserService` to demonstrate the protected-endpoint + identity-resolution flow end-to-end.
- `FinBean.Core`: added `ICurrentUserService` abstraction (`IsAuthenticated`, `UserId` from the JWT `sub`/`NameIdentifier` claim); concrete `CurrentUserService` lives in `FinBean.Api` (reads only from validated `HttpContext.User` claims, never headers/input).
- OpenAPI exposed at `/openapi/v1.json` (Development only, via `MapOpenApi()`).
- Packages added: `Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.3, `Microsoft.EntityFrameworkCore.Design` 10.0.4, `Microsoft.AspNetCore.Authentication.JwtBearer` 10.0.10, `Microsoft.AspNetCore.OpenApi` 10.0.10, and a direct `Microsoft.OpenApi` 2.7.5 override to remediate a vulnerable transitive `Microsoft.OpenApi` 2.0.0 (NU1903/GHSA-v5pm-xwqc-g5wc).
- Security review: no blocking issues. Low/info items recorded for later hardening (HSTS in non-dev, tighten `AllowedHosts`, explicit CORS allow-list once a frontend origin exists).
- QA found and a fix resolved one critical bug: the `finbean-frontend` Keycloak client had no audience mapper, so real tokens carried `aud: "account"` and were rejected by the backend's `ValidAudience: "finbean-frontend"` check. Fixed by adding an `oidc-audience-mapper` protocol mapper to `docker/keycloak/realm-export.json` (backend C# code was not changed). Re-verified: real tokens now pass, unauthenticated requests still correctly return 401 ProblemDetails, identity resolution confirmed end-to-end.
- "All data access is scoped to the authenticated user" is not yet independently testable — no domain entities/DbSets exist in this slice; the `ICurrentUserService` building block is in place for later features (e.g. salary/expense tracking) to use for per-user scoping.
- Manual local setup: `docker compose -f docker/docker-compose.yml up -d`, then `dotnet ef database update --project backend/FinBean.Database/FinBean.Database --startup-project backend/FinBean.Database/FinBean.Database` to apply migrations. Config values (DB connection string, Keycloak authority/audience) are supplied via `appsettings.Development.json` (mirroring the docker-compose dev defaults) or `dotnet user-secrets` for developer-specific overrides.
