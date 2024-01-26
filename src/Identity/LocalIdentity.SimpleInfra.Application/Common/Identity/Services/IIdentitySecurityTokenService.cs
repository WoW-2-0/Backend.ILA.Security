using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

/// <summary>
/// Defines identity security token functionalities
/// </summary>
public interface IIdentitySecurityTokenService
{
    /// <summary>
    /// Creates access token for given user
    /// </summary>
    /// <param name="accessToken">Access token to create</param>
    /// <param name="saveChanges">Determines whether to send changes to data source</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created access token</returns>
    ValueTask<AccessToken> CreateAccessTokenAsync(AccessToken accessToken, bool saveChanges = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets access token by given id
    /// </summary>
    /// <param name="accessTokenId">Access token Id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Access token if found, otherwise null</returns>
    ValueTask<AccessToken?> GetAccessTokenByIdAsync(Guid accessTokenId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates refresh token for given user
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <param name="saveChanges">Determines whether to send changes to data source</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created refresh token</returns>
    ValueTask<RefreshToken> CreateRefreshTokenAsync(
        RefreshToken refreshToken,
        bool saveChanges = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets refresh token by given token value
    /// </summary>
    /// <param name="refreshTokenValue">Refresh token value</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Refresh token if found, otherwise null</returns>
    ValueTask<RefreshToken?> GetRefreshTokenByValueAsync(string refreshTokenValue, CancellationToken cancellationToken = default);
    
    ValueTask RemoveRefreshTokenAsync(string refreshTokenValue, CancellationToken cancellationToken = default);
    
    ValueTask RemoveAccessTokenAsync(Guid accessTokenId, CancellationToken cancellationToken = default);
}