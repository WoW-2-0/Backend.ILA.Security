using System.Linq.Expressions;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Persistence.DataContexts;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;

namespace LocalIdentity.SimpleInfra.Persistence.Repositories;

public class UserSignInDetailsRepository(IdentityDbContext dbContext) : EntityRepositoryBase<UserSignInDetails, IdentityDbContext>(dbContext), IUserSignInDetailsRepository
{
    public IQueryable<UserSignInDetails> Get(Expression<Func<UserSignInDetails, bool>>? predicate = default, bool asNoTracking = false) =>
        base.Get(predicate, asNoTracking);

    public ValueTask<UserSignInDetails> CreateAsync(UserSignInDetails userSignInDetails, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.CreateAsync(userSignInDetails, saveChanges, cancellationToken);
}