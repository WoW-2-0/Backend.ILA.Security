using System.Linq.Expressions;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Persistence.DataContexts;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Persistence.Repositories;

public class AccessTokenRepository(IdentityDbContext dbContext) : EntityRepositoryBase<AccessToken, IdentityDbContext>(dbContext),
    IAccessTokenRepository
{
    public new IQueryable<AccessToken> Get(Expression<Func<AccessToken, bool>>? predicate = default, bool asNoTracking = false) =>
        base.Get(predicate, asNoTracking);

    public new ValueTask<AccessToken?> GetByIdAsync(Guid accessTokenId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    => base.GetByIdAsync(accessTokenId, asNoTracking, cancellationToken);

    public new ValueTask<AccessToken> CreateAsync(AccessToken accessToken, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.CreateAsync(accessToken, saveChanges, cancellationToken);

    public new ValueTask<AccessToken> UpdateAsync(AccessToken accessToken, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(accessToken, saveChanges, cancellationToken);

    public async ValueTask RevokeAllAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await DbContext.AccessTokens.Where(accessToken => accessToken.UserId == userId)
            .ExecuteUpdateAsync(accessToken => accessToken.SetProperty(token => token.IsRevoked, true), cancellationToken: cancellationToken);
    }
}