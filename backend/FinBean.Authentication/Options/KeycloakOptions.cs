using System.ComponentModel.DataAnnotations;

namespace FinBean.Authentication.Options;

public class KeycloakOptions
{
    [Required]
    public required string Authority { get; init; }

    [Required]
    public required string Audience { get; init; }

    /// <summary>
    /// Whether HTTPS is required for retrieving the OIDC metadata/signing keys from the authority.
    /// Should be <c>true</c> in production; only disabled for local Keycloak instances running over HTTP.
    /// </summary>
    public bool RequireHttpsMetadata { get; init; } = true;
}
