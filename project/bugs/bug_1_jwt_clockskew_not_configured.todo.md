# JWT clock skew not explicitly configured

## Description

`TokenValidationParameters` in `backend/FinBean.Api/AuthenticationDi.cs` sets `ValidateLifetime = true` but never sets `ClockSkew`, so .NET's default 5-minute tolerance applies. This is not an auth bypass (expiry is still enforced), but it means an expired/revoked token stays usable for up to ~5 extra minutes, which is a meaningfully wide window for a financial app.

Also related: `ValidateIssuerSigningKey` and `ValidAlgorithms` are not explicitly set on `TokenValidationParameters` (relying on framework defaults). Signature checking still happens, but should be made explicit per JWT hardening best practice.

## How to reproduce

1. Set the Keycloak realm's `accessTokenLifespan` to a very short value (e.g. 2s) via the admin API.
2. Obtain a token and call a protected endpoint immediately (succeeds, as expected).
3. Wait progressively longer and keep calling the endpoint with the same token.
4. Observe the endpoint keeps returning 200 for up to ~5 minutes after the token's `exp`, only returning 401 once ~5 minutes have elapsed since expiry.

## Acceptance criteria

- `ClockSkew` is explicitly set to a small value (e.g. 30 seconds) on `TokenValidationParameters` in `AuthenticationDi.cs`.
- `ValidateIssuerSigningKey = true` and an explicit `ValidAlgorithms` allow-list are set on `TokenValidationParameters`.
- A token becomes rejected (401) within the configured clock skew window after its `exp`, not ~5 minutes later.

## implementation
