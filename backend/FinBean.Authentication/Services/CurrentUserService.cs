using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace FinBean.Authentication.Services;

/// <summary>
/// Resolves the identity of the currently authenticated user from validated request claims.
/// Implementations must never trust request input/headers directly.
/// </summary>
public class CurrentUserService(IHttpContextAccessor httpContextAccessor)
{
    
    /// <summary>
    /// Whether the current request has a validated, authenticated user.
    /// </summary>
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    /// <summary>
    /// The authenticated user's identifier (the JWT `sub`/NameIdentifier claim), or <c>null</c> if unauthenticated.
    /// </summary>
    public string? UserId =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? httpContextAccessor.HttpContext?.User.FindFirstValue("sub");
}
