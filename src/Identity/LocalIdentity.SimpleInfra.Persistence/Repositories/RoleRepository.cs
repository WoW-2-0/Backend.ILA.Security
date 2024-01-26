using System.Linq.Expressions;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Persistence.DataContexts;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;

namespace LocalIdentity.SimpleInfra.Persistence.Repositories;

public class RoleRepository(IdentityDbContext dbContext) : EntityRepositoryBase<Role, IdentityDbContext>(dbContext), IRoleRepository
{
    public IQueryable<Role> Get(Expression<Func<Role, bool>>? predicate = default, bool asNoTracking = false) => base.Get(predicate, asNoTracking);
}