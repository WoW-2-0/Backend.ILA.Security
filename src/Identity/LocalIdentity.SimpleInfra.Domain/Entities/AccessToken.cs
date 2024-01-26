using LocalIdentity.SimpleInfra.Domain.Common.Entities;

namespace LocalIdentity.SimpleInfra.Domain.Entities;

/// <summary>
/// Represents access token
/// </summary>
public class AccessToken : TokenBase
{
    public AccessToken()
    {
    }

    public AccessToken(Guid id, Guid userId, string token, DateTimeOffset expiryTime, bool isRevoked)
    {
        Id = id;
        UserId = userId;
        Token = token;
        ExpiryTime = expiryTime;
        IsRevoked = isRevoked;
    }

    /// <summary>
    /// Gets or sets a value indicating whether token is revoked
    /// </summary>
    public bool IsRevoked { get; set; }
}