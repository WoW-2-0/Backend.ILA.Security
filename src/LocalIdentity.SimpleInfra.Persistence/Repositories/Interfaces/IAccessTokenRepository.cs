using System.Linq.Expressions;
using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;

public interface IAccessTokenRepository
{
    IQueryable<AccessToken> Get(Expression<Func<AccessToken, bool>>? predicate = default, bool asNoTracking = false);

    ValueTask<AccessToken?> GetByIdAsync(Guid accessTokenId, bool asNoTracking = false, CancellationToken cancellationToken = default);
    
    ValueTask<AccessToken> CreateAsync(AccessToken accessToken, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<AccessToken> UpdateAsync(AccessToken accessToken, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask RevokeAllAsync(Guid userId, CancellationToken cancellationToken = default);
}