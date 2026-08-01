# Code Review: Feature 2 — Backend Foundation & Authenticated API

**Branch**: `feature/backend_foundation` (diff against `develop`)
**Scope**: `backend/FinBean.Api` (auth wiring, exception handling, `UserProfileController`), `backend/FinBean.Core` (`ICurrentUserService`), `backend/FinBean.Database` (`UserProfile` model + migration), `appsettings*.json`
**Ready for Production**: No (1 High finding must be fixed first)
**Safe to proceed to QA testing loop**: Yes — the High finding does not affect correctness of local/Development QA testing, but must be fixed before merging to `develop`/deploying, per `project/architecture.md`'s zero-trust/auth requirements.
**Critical/Blocking Issues**: 0
**High Issues**: 1

## Priority 1 (Must Fix before merge/production) ⛔

### 1. `RequireHttpsMetadata: false` is set in the shared base `appsettings.json`, not scoped to Development only — High (OWASP A02 Cryptographic Failures / A07 Identification & Authentication Failures)

`backend/FinBean.Api/appsettings.json`:
```json
"KeycloakOptions": {
  "Authority": "http://localhost:8080/realms/finbean",
  "Audience": "finbean-frontend",
  "RequireHttpsMetadata": false
}
```

`KeycloakOptions.RequireHttpsMetadata` defaults to `true` in C# (`Options/KeycloakOptions.cs`), which is correct — but the base `appsettings.json` (loaded in **every** environment, including Production, before any environment-specific file) explicitly overrides it to `false`. There is no `appsettings.Production.json` in the repo to re-enable it. If this app is ever deployed with only environment variables for `Authority`/`Audience` (a very plausible pattern), `RequireHttpsMetadata` silently stays `false` in production because the base file already disabled it.

**Impact**: With `RequireHttpsMetadata=false`, the JWT bearer handler fetches OIDC discovery metadata and JWKS signing keys over plain HTTP. An attacker with a network position (MITM, DNS spoofing, compromised proxy) can serve forged signing keys, allowing them to mint tokens the API will accept as valid — a full authentication bypass.

**Fix**: Remove `"RequireHttpsMetadata": false` from `appsettings.json` (let it fall back to the secure `true` default), and keep the `false` override only in `appsettings.Development.json` (already present there). Optionally add an explicit `"RequireHttpsMetadata": true` in `appsettings.json` for clarity/defense-in-depth.

## Recommended Changes (Medium) 🟡

### 2. JWT `TokenValidationParameters` doesn't explicitly set `ValidateIssuerSigningKey` or restrict algorithms

`backend/FinBean.Api/AuthenticationDi.cs`:
```csharp
jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = options.Authority,
    ValidateAudience = true,
    ValidAudience = options.Audience,
    ValidateLifetime = true,
};
```
Signature verification still occurs (JWKS keys are supplied automatically via the `Authority`/`ConfigurationManager`, and `RequireSignedTokens` defaults to `true`), so this is not a bypass. But best practice / OWASP JWT guidance is to make all validation intents explicit and auditable:
- Add `ValidateIssuerSigningKey = true` explicitly.
- Add `ValidAlgorithms = ["RS256"]` (or whatever Keycloak's realm actually signs with) to close off any theoretical algorithm-confusion attack vector and prevent silently accepting a future weaker algorithm.

### 3. Controller returns the raw EF Core entity instead of a DTO

`backend/FinBean.Api/Controllers/UserProfileController.cs`:
```csharp
return Ok(userProfiles);
```
Returns the `UserProfile` entity directly, exposing the internal `KeycloakUserId` (Keycloak subject GUID) in the API response. Not exploitable today (it's only the caller's own id, and reads are correctly scoped — see below), but:
- It establishes a pattern that will lead to over-posting/mass-assignment risk once `POST`/`PUT` endpoints are added directly against the same entity.
- It leaks an internal identifier unnecessarily.

Recommend introducing a response DTO (e.g., `UserProfileResponse { Id, Username }`) now, before more endpoints are built on this controller.

## Low / Informational 🔵

- No `app.UseHsts()` for non-Development environments — only `UseHttpsRedirection()` is present. Add `if (!app.Environment.IsDevelopment()) { app.UseHsts(); }`.
- `AllowedHosts: "*"` is unchanged scaffold default; tighten before production.
- No CORS policy configured yet. Not a defect in this diff (no cross-origin caller exists yet), but flagging since auth is now enforced — an explicit origin allow-list (not `AllowAnyOrigin`) will be needed once the frontend calls this API cross-origin.
- `CurrentUserService.UserId` falls back from `ClaimTypes.NameIdentifier` to the raw `"sub"` claim. Harmless (the JWT handler already auto-maps `sub` → `NameIdentifier`), just redundant.

## Verified as Correct ✅

- **JWT validation**: `ValidateIssuer`, `ValidateAudience`, `ValidateLifetime` are all enabled and bound via the options pattern (`KeycloakOptions`), not read from raw `IConfiguration` in business code. Matches `project/architecture.md`'s authentication requirements.
- **User identity resolution**: `CurrentUserService.UserId` is read exclusively from the validated `HttpContext.User` (`ClaimsPrincipal`) after successful JWT signature/issuer/audience/lifetime validation — never from request headers/body/query. Not spoofable by a caller.
- **Per-user data scoping / IDOR**: `UserProfileController.GetUserProfiles()` filters by `KeycloakUserId == _currentUserService.UserId`, correctly preventing cross-user access. No endpoint accepts a caller-supplied user id.
- **Error handling / information disclosure**: `GlobalExceptionHandler` only sets `ProblemDetails.Detail = exception.ToString()` when `environment.IsDevelopment()`; all other environments get a generic message. No stack traces or exception messages leak in non-dev. Log statements only include method/path, not headers or bodies (no token leakage into logs).
- **Injection**: The EF Core LINQ query in `UserProfileController` is fully parameterized (no raw SQL/string concatenation) — no SQL injection risk.
- **Dependency CVE remediation**: The `Microsoft.OpenApi` package is pinned directly to `2.7.5` in `FinBean.Api.csproj`, overriding the vulnerable transitive `2.0.0` version that triggered NU1903/GHSA-v5pm-xwqc-g5wc. Confirmed as a direct top-level override, which is the correct remediation approach.
- **Migration**: `20260801101357_InitialCreate` matches the `FinBeanDatabaseContext` model exactly (unique index on `KeycloakUserId`); no drift between model and migration.

## Verdict

Not production-ready until Finding #1 is fixed (trivial one-line config change). Safe to hand off to the QA testing loop in the meantime, since QA will run against the Development configuration where the HTTPS-metadata relaxation is intentional and does not affect functional test outcomes. Recommend fixing #1 before the branch is merged into `develop`, and addressing #2/#3 as follow-up hardening (can be same PR or fast-follow).
