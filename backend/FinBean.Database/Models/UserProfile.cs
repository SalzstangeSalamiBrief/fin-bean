namespace FinBean.Database.Models;

public class UserProfile
{
    public int Id { get; set; }

    public string Username { get; set; }

    /// <summary>
    /// The Keycloak subject (`sub` claim) identifying the owning user. Used to scope all
    /// data access to the authenticated user.
    /// </summary>
    public string KeycloakUserId { get; set; }
}