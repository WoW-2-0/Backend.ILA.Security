using FluentValidation;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class AccessTokenService(IValidator<AccessToken> accessTokenValidator, IAccessTokenRepository accessTokenRepository) : IAccessTokenService
{
    public async ValueTask<AccessToken> CreateAsync(AccessToken accessToken, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var validationResult = accessTokenValidator.Validate(accessToken, options => options.IncludeRuleSets(EntityEvent.OnCreate.ToString()));
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        await accessTokenRepository.RevokeAllAsync(accessToken.UserId, cancellationToken: cancellationToken);
        return await accessTokenRepository.CreateAsync(accessToken, saveChanges, cancellationToken);
    }

    public async ValueTask<AccessToken?> GetLastAsync(Guid userId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return await accessTokenRepository.Get(accessToken => accessToken.UserId == userId)
            .OrderByDescending(accessToken => accessToken.CreatedTime)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async ValueTask RevokeAllTokenAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        await accessTokenRepository.RevokeAllAsync(userId, cancellationToken: cancellationToken);
    }
}