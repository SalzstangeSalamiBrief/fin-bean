# GDPR cascade delete

## Description

When a user's Keycloak account is deleted, all of their app data must be removed for GDPR compliance. This includes expenses, goals, spending profile, salary and profile settings.

## Acceptance criteria

- Deleting the Keycloak account triggers deletion of all app data owned by that user.
- Expenses, goals, spending profile, salary and settings are all removed.
- No orphaned user data remains in the database after deletion.

## implementation
