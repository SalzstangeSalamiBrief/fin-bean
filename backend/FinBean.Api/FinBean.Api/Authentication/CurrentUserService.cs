using System.Security.Claims;
using FinBean.Core.Authentication;

namespace FinBean.Api.Authentication;

/// <summary>
/// Resolves the current user's identity from the <see cref="HttpContext"/> of the active request,
/// based on claims populated by JWT bearer authentication (see <c>Program.ConfigureJwtBearer</c>).
/// </summary>
public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public string UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            // Depending on JwtBearerOptions.MapInboundClaims, the "sub" claim may or may not be
            // remapped to ClaimTypes.NameIdentifier, so both are checked defensively.
            var subject = user?.FindFirstValue(ClaimTypes.NameIdentifier) ?? user?.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new InvalidOperationException("No authenticated user is available on the current request.");
            }

            return subject;
        }
    }
}
