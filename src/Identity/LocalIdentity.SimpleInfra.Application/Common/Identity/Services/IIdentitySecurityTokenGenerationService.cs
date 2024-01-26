using System.Security.Claims;
using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

/// <summary>
/// Defines identity security token generation functionalities
/// </summary>
public interface IIdentitySecurityTokenGenerationService
{
    /// <summary>
    /// Generates access token for given user
    /// </summary>
    /// <param name="user">User to create access token for</param>
    /// <returns>A new instance of access token</returns>
    AccessToken GenerateAccessToken(User user);

    /// <summary>
    /// Generates refresh token for given user
    /// </summary>
    /// <param name="user">User to create access token for</param>
    /// <returns>A new instance of refresh token</returns>
    IdentitySecurityToken GenerateRefreshToken(User user);

    /// <summary>
    /// Gets claims principal from given token value
    /// </summary>
    /// <param name="tokenValue">Valid access token value</param>
    /// <returns>Claims principal</returns>
    ClaimsPrincipal? GetPrincipal(string tokenValue);
}