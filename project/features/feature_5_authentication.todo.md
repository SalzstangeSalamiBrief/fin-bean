# Authentication & user management

## Description

End-to-end authentication and user account lifecycle handled via Keycloak: register, log in, log out, update profile credentials, and delete account. The frontend drives the Keycloak login/logout and account flows, the backend trusts Keycloak-issued tokens (see backend foundation), and each authenticated user is mapped to their app data. A user can only see and manipulate their own data. Single-user accounts only (no shared/family accounts). Account deletion coordinates with the GDPR cascade delete of app data.

## Acceptance criteria

- User can register a new account via Keycloak.
- User can log in and log out; the session/token lifecycle (issue + refresh) works.
- User can update their account (e.g. credentials/email) via Keycloak.
- User can delete their account; deletion triggers the GDPR cascade delete of their app data.
- The authenticated identity is consistently mapped to the app user across frontend and backend.
- Users can only access and manipulate their own data; unauthenticated access is rejected.

## implementation
