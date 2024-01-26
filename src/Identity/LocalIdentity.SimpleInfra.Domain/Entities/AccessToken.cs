namespace LocalIdentity.SimpleInfra.Domain.Entities;

/// <summary>
/// Represents access token
/// </summary>
public class AccessToken : IdentitySecurityToken
{
    public AccessToken()
    {
    }

    public AccessToken(Guid id, Guid userId, DateTimeOffset expiryTime) => (Id, UserId, ExpiryTime) = (id, userId, expiryTime);
}