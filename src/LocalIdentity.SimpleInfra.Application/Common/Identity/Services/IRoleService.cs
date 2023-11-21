using LocalIdentity.SimpleInfra.Application.Common.Querying;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

public interface IRoleService
{
    ValueTask<IList<Role>> GetByFilterAsync(
        FilterPagination filterOptions,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    );

    ValueTask<Role?> GetByTypeAsync(RoleType roleType, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<Guid> GetDefaultRoleId(CancellationToken cancellationToken = default);
}