using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Constants;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Extensions;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class IdentitySecurityTokenGenerationService(IOptions<IdentityTokenSettings> jwtSettings) : IIdentitySecurityTokenGenerationService
{
    private readonly IdentityTokenSettings _identityTokenSettings = jwtSettings.Value;

    public AccessToken GenerateAccessToken(User user)
    {
        var accessToken = new AccessToken(Guid.NewGuid(), user.Id, DateTime.UtcNow.AddMinutes(_identityTokenSettings.ExpirationTimeInMinutes));
        var jwtToken = GetJwtToken(user, accessToken);
        accessToken.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return accessToken;
    }

    public RefreshToken GenerateRefreshToken(User user, bool extendedExpiryTime = false)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = Convert.ToBase64String(randomNumber),
            UserId = user.Id,
            ExpiryTime = DateTime.UtcNow.AddMinutes(
                extendedExpiryTime
                    ? _identityTokenSettings.RefreshTokenExtendedExpirationTimeInMinutes
                    : _identityTokenSettings.RefreshTokenExpirationTimeInMinutes
            )
        };
    }

    public AccessToken? GetAccessToken(string tokenValue)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var getAccessToken = () =>
        {
            var tokenWithoutPrefix = tokenValue.Replace("Bearer ", string.Empty);
            var principal = tokenHandler.ValidateToken(tokenWithoutPrefix, _identityTokenSettings.MapToTokenValidationParameters(), out var validatedToken);

            // Additional validation to ensure the token is the type we are expecting
            if (validatedToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase
                ))
                throw new SecurityTokenException("Invalid token");

            return new AccessToken
            {
                Id = Guid.Parse(principal.FindFirst(JwtRegisteredClaimNames.Jti)!.Value),
                UserId = Guid.Parse(principal.FindFirst(ClaimConstants.UserId)!.Value),
                Token = tokenValue,
                ExpiryTime = jwtSecurityToken.ValidTo.ToUniversalTime()
            };
        };

        return getAccessToken.GetValue().Data;
    }

    private JwtSecurityToken GetJwtToken(User user, AccessToken accessToken)
    {
        var claims = GetClaims(user, accessToken);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_identityTokenSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: _identityTokenSettings.ValidIssuer,
            audience: _identityTokenSettings.ValidAudience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: accessToken.ExpiryTime.DateTime,
            signingCredentials: credentials
        );
    }

    private List<Claim> GetClaims(User user, AccessToken accessToken)
    {
        return
        [
            new Claim(ClaimTypes.Email, user.EmailAddress),
            new Claim(ClaimTypes.Role, user.Role!.Type.ToString()),
            new Claim(ClaimConstants.UserId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, accessToken.Id.ToString())
        ];
    }
}