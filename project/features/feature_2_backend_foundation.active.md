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
