using LocalIdentity.SimpleInfra.Domain.Common.Entities;

namespace LocalIdentity.SimpleInfra.Domain.Entities;

/// <summary>
/// Represents access token
/// </summary>
public class AccessToken : IdentitySecurityToken
{
    public AccessToken()
    {
    }

    public AccessToken(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets or sets a value indicating whether token is revoked
    /// </summary>
    public bool IsRevoked { get; set; }
}