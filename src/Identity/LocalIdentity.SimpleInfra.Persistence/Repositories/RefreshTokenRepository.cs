using LocalIdentity.SimpleInfra.Domain.Common.Caching;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Persistence.Caching.Brokers;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;

namespace LocalIdentity.SimpleInfra.Persistence.Repositories;

public class RefreshTokenRepository(ICacheBroker cacheBroker) : IRefreshTokenRepository
{
    public async ValueTask<RefreshToken> CreateAsync(RefreshToken refreshToken, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var cacheEntryOptions = new CacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = refreshToken.ExpiryTime - DateTimeOffset.UtcNow
        };

        await cacheBroker.SetAsync($"{nameof(RefreshToken)}-{refreshToken.Token}", refreshToken, cacheEntryOptions, cancellationToken);

        return refreshToken;
    }

    public ValueTask<RefreshToken?> GetByValueAsync(string refreshTokenValue, CancellationToken cancellationToken = default)
    {
        return cacheBroker.GetAsync<RefreshToken>($"{nameof(RefreshToken)}-{refreshTokenValue}", cancellationToken: cancellationToken);
    }

    public ValueTask RemoveAsync(string refreshTokenValue, CancellationToken cancellationToken = default)
    {
        return cacheBroker.DeleteAsync($"{nameof(RefreshToken)}-{refreshTokenValue}", cancellationToken: cancellationToken);
    }
}