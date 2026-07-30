namespace FinBean.Core.Authentication;

/// <summary>
/// Resolves the identity of the currently authenticated user from the incoming request.
/// All data access must be scoped using <see cref="UserId"/> to prevent cross-user access.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets a value indicating whether the current request has an authenticated user.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Gets the authenticated user's identifier (the "sub" claim of the validated JWT).
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when there is no authenticated user.</exception>
    string UserId { get; }
}
