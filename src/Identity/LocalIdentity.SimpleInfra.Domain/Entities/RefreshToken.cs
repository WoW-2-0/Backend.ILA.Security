namespace LocalIdentity.SimpleInfra.Domain.Entities;

/// <summary>
/// Represents refresh token
/// </summary>
public class RefreshToken : IdentitySecurityToken
{
    public bool EnableExtendedExpiryTime { get; set; }
}
