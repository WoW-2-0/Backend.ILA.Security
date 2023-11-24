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

    public ValueTask<AccessToken?> GetByIdAsync(Guid accessTokenId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return accessTokenRepository.GetByIdAsync(accessTokenId, asNoTracking, cancellationToken);
    }

    public async ValueTask<AccessToken?> GetLastAsync(Guid userId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return await accessTokenRepository.Get(accessToken => accessToken.UserId == userId)
            .OrderByDescending(accessToken => accessToken.CreatedTime)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async ValueTask RevokeAllTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await accessTokenRepository.RevokeAllAsync(userId, cancellationToken: cancellationToken);
    }

    public async ValueTask<AccessToken> UpdateAsync(AccessToken accessToken, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var foundAccessToken = await accessTokenRepository.GetByIdAsync(accessToken.Id, cancellationToken: cancellationToken) ??
                               throw new InvalidOperationException("Access token not found");

        var selector = ValidatorOptions.Global.ValidatorSelectors.RulesetValidatorSelectorFactory(new[] { EntityEvent.OnUpdate.ToString() });
        var validationContext = new ValidationContext<AccessToken>(accessToken, null, selector);
        validationContext.RootContextData.Add(nameof(AccessToken), foundAccessToken);
        var validationResult = accessTokenValidator.Validate(validationContext);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        foundAccessToken.Token = accessToken.Token;
        foundAccessToken.ExpiryTime = accessToken.ExpiryTime;

        return await accessTokenRepository.UpdateAsync(foundAccessToken, saveChanges, cancellationToken);
    }

    public async ValueTask RevokeAsync(Guid accessTokenId, bool saveChanges, CancellationToken cancellationToken = default)
    {
        var foundAccessToken = await accessTokenRepository.GetByIdAsync(accessTokenId, cancellationToken: cancellationToken) ??
                               throw new InvalidOperationException("Access token not found");

        foundAccessToken.IsRevoked = true;

        await accessTokenRepository.UpdateAsync(foundAccessToken, saveChanges, cancellationToken);
    }
}