using FluentValidation;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class IdentitySecurityTokenService(
    IAccessTokenRepository accessTokenRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IValidator<AccessToken> accessTokenValidator,
    IValidator<RefreshToken> refreshTokenValidator
) : IIdentitySecurityTokenService
{
    public ValueTask<AccessToken> CreateAccessTokenAsync(
        AccessToken accessToken,
        bool saveChanges = true,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = accessTokenValidator.Validate(accessToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return accessTokenRepository.CreateAsync(accessToken, saveChanges, cancellationToken);
    }

    public ValueTask<AccessToken?> GetAccessTokenByIdAsync(Guid accessTokenId, CancellationToken cancellationToken = default)
    {
        return accessTokenRepository.GetByIdAsync(accessTokenId, cancellationToken);
    }

    public ValueTask<RefreshToken> CreateRefreshTokenAsync(
        RefreshToken refreshToken,
        bool saveChanges = true,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = refreshTokenValidator.Validate(refreshToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return refreshTokenRepository.CreateAsync(refreshToken, saveChanges, cancellationToken);
    }

    public ValueTask<RefreshToken?> GetRefreshTokenByValueAsync(string refreshTokenValue, CancellationToken cancellationToken = default)
    {
        return refreshTokenRepository.GetByValueAsync(refreshTokenValue, cancellationToken);
    }

    public ValueTask RemoveRefreshTokenAsync(string refreshTokenValue, CancellationToken cancellationToken = default)
    {
        return refreshTokenRepository.RemoveAsync(refreshTokenValue, cancellationToken);
    }

    public ValueTask RemoveAccessTokenAsync(Guid accessTokenId, CancellationToken cancellationToken = default)
    {
        return accessTokenRepository.RemoveAsync(accessTokenId, cancellationToken);
    }
}