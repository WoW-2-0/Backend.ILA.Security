using System.Formats.Asn1;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Application.Common.Querying;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class RoleService(IRoleRepository roleRepository) : IRoleService
{
    public async ValueTask<IList<Role>> GetByFilterAsync(
        FilterPagination filterPagination,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return await roleRepository.Get(asNoTracking: asNoTracking)
            .Skip(filterPagination.PageToken)
            .Take(filterPagination.PageSize)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async ValueTask<Role?> GetByTypeAsync(RoleType roleType, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return await roleRepository.Get(role => role.Type == roleType, asNoTracking: asNoTracking).FirstOrDefaultAsync(cancellationToken);
    }
}