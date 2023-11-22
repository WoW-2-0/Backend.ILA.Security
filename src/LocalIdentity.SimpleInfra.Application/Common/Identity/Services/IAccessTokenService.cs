using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

public interface IAccessTokenService
{
    ValueTask<AccessToken> CreateAsync(
        Guid userId, 
        string value,
        DateTimeOffset expiryTime,
        bool saveChanges = true,
        CancellationToken cancellationToken = default
    );
    
    ValueTask<AccessToken?> GetLastAsync(Guid userId, bool asNoTracking = false, CancellationToken cancellationToken = default);
}