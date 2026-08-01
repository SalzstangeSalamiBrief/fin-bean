# UserProfile lacks a response DTO and has nullability foot-guns

## Description

`UserProfileController.GetUserProfiles` returns the raw EF Core `UserProfile` entity directly, which exposes the internal `KeycloakUserId` column to API consumers and sets a bad precedent once write endpoints are added (over-posting risk). Separately, `UserProfile.Username` and `UserProfile.KeycloakUserId` are non-nullable reference types without a `required` modifier or constructor initialization, producing `CS8618` warnings on every build.

## How to reproduce

1. Build the solution (`dotnet build backend/FinBeanBackendSolution.sln`) and observe `CS8618` warnings for `UserProfile.Username`/`KeycloakUserId`.
2. Call `GET /UserProfile/GetUserProfiles` with a valid token and observe the response body includes `keycloakUserId`, an internal identifier that callers shouldn't need or see.

## Acceptance criteria

- A dedicated response DTO (e.g. `UserProfileResponse`) is returned from `UserProfileController` instead of the raw `UserProfile` entity, excluding `KeycloakUserId`.
- `UserProfile.Username` and `UserProfile.KeycloakUserId` use the `required` modifier (matching the existing pattern in `KeycloakOptions`), and the `CS8618` warnings are resolved.
- Build produces no `CS8618` warnings for `UserProfile`.

## implementation
