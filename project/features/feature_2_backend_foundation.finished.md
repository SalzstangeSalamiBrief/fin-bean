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

- `FinBean.Api`: added Keycloak JWT bearer authentication via a new `AuthenticationDi.cs` (DI-class-per-project convention) — `AddKeycloakOptions()` binds a `KeycloakOptions` (`Authority`, `Audience`, `RequireHttpsMetadata`) via the options pattern with data-annotation validation; `AddKeycloakAuthentication()` configures `JwtBearerOptions` validating issuer/audience/lifetime from those options. `UseAuthentication()` added before `UseAuthorization()`. `RequireHttpsMetadata: false` is set only in `appsettings.Development.json` (defaults to secure `true` otherwise). Added `AddProblemDetails()` + a `GlobalExceptionHandler` (`IExceptionHandler`) so all errors return ProblemDetails, with exception detail gated to Development via `IHostEnvironment`.
- `FinBean.Core`: added `ICurrentUserService` (`IsAuthenticated`, `UserId`); concrete `CurrentUserService` in `FinBean.Api` reads only validated `HttpContext.User` claims (`NameIdentifier`/`sub`), never headers/input.
- `FinBean.Database`: added a `KeycloakUserId` column (unique-indexed) on `UserProfile` to link a profile to its Keycloak identity, generated a real `InitialCreate` EF Core migration, and verified `dotnet ef database update` applies cleanly against live PostgreSQL.
- `UserProfileController`: added `[Authorize]`, removed the previous unscoped `// TODO REMOVE LATER` query; `GetUserProfiles` now filters by `KeycloakUserId == currentUser.UserId` so no cross-user data is returned.
- OpenAPI exposed via `MapOpenApi()` (Development) + Scalar UI; `GetUserProfiles` response schema fixed to `List<UserProfile>` so Orval generates an array type, not a single object.
- Packages added: `Microsoft.AspNetCore.Authentication.JwtBearer` 10.0.10, `Microsoft.EntityFrameworkCore.Design` 10.0.10, and a direct `Microsoft.OpenApi` 2.7.5 pin remediating a transitive CVE (NU1903/GHSA-v5pm-xwqc-g5wc).
- Security review: 1 High finding (`RequireHttpsMetadata: false` was set in the base `appsettings.json`, applying to all environments including production) — fixed by scoping it to `appsettings.Development.json` only, re-verified. 2 Medium/4 Low items (explicit `ValidateIssuerSigningKey`/`ValidAlgorithms`, response DTO instead of raw entity, missing HSTS/CORS/AllowedHosts hardening) tracked as follow-ups, not blocking.
- QA: verified build, live migration apply against PostgreSQL, real Keycloak token accepted (password grant, realm `finbean`, client `finbean-frontend`), malformed/tampered tokens rejected (401), unauthenticated requests return 401 ProblemDetails, per-user data scoping confirmed with two distinct Keycloak users (no cross-user leakage), 401/404/500 all return valid ProblemDetails bodies, OpenAPI document valid. One High bug found (OpenAPI schema documented a single object instead of an array for `GetUserProfiles`, would have broken Orval codegen) — fixed and re-verified.
- Follow-ups tracked as bugs (non-blocking): [bug_1_jwt_clockskew_not_configured.todo.md](../bugs/bug_1_jwt_clockskew_not_configured.todo.md), [bug_2_userprofile_response_dto_and_nullability.todo.md](../bugs/bug_2_userprofile_response_dto_and_nullability.todo.md).
