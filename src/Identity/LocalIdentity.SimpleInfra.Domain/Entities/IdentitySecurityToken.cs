using LocalIdentity.SimpleInfra.Domain.Common.Entities;

namespace LocalIdentity.SimpleInfra.Domain.Entities;

/// <summary>
/// Represents security token
/// </summary>
public abstract class IdentitySecurityToken : Entity
{
    /// <summary>
    /// Gets or sets user Id
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Gets or sets actual token value
    /// </summary>
    public string Token { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets token refresh time
    /// </summary>
    public DateTimeOffset ExpiryTime { get; set; }
}