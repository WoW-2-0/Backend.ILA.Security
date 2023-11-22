using LocalIdentity.SimpleInfra.Domain.Common;
using LocalIdentity.SimpleInfra.Domain.Common.Entities;

namespace LocalIdentity.SimpleInfra.Domain.Entities;

public class AccessToken : AuditableEntity
{
    public string Token { get; set; } = default!;

    public Guid UserId { get; set; }

    public bool IsRevoked { get; set; }

    public DateTimeOffset ExpiryTime { get; set; }
}