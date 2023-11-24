using LocalIdentity.SimpleInfra.Domain.Common.Entities;

namespace LocalIdentity.SimpleInfra.Domain.Entities;

public class AccessToken : AuditableEntity
{
    public AccessToken()
    {
    }

    public AccessToken(Guid userId)
    {
        Token = string.Empty;
        ExpiryTime = DateTimeOffset.UtcNow;
        UserId = userId;
    }

    public string Token { get; set; } = default!;

    public DateTimeOffset ExpiryTime { get; set; }

    public bool IsRevoked { get; set; }

    public Guid UserId { get; set; }
}