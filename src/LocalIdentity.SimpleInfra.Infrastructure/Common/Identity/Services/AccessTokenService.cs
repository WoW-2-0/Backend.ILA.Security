using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class AccessTokenService : IAccessTokenService
{
    public ValueTask<AccessToken> CreateAsync(Guid userId, string value, DateTimeOffset expiryTime, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<AccessToken?> GetLastAsync(Guid userId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}