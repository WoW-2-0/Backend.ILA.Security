using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

public interface IAccessTokenService
{
    ValueTask<AccessToken> CreateAsync(AccessToken accessToken, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<AccessToken?> GetLastAsync(Guid userId, bool asNoTracking = false, CancellationToken cancellationToken = default);
        
    ValueTask RevokeAllTokenAsync(Guid userId  , CancellationToken cancellationToken = default);
}