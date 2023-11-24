using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

public interface IAccessTokenService
{
    ValueTask<AccessToken> CreateAsync(AccessToken accessToken, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<AccessToken?> GetByIdAsync(Guid accessTokenId, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask RevokeAsync(Guid accessTokenId, bool saveChanges, CancellationToken cancellationToken = default);

    ValueTask<AccessToken> UpdateAsync(AccessToken accessToken, bool saveChanges = true, CancellationToken cancellationToken = default);
}