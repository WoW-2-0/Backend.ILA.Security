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

public class IdentitySecurityTokenGenerationService(IOptions<JwtSettings> jwtSettings) : IIdentitySecurityTokenGenerationService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public AccessToken GenerateAccessToken(User user)
    {
        var accessToken = new AccessToken(Guid.NewGuid());
        var jwtToken = GetJwtToken(user, accessToken);
        accessToken.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return accessToken;
    }

    public IdentitySecurityToken GenerateRefreshToken(User user)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber)
        };
    }

    public ClaimsPrincipal? GetPrincipal(string tokenValue)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var getPrincipal = () =>
        {
            var principal = tokenHandler.ValidateToken(tokenValue, _jwtSettings.MapToTokenValidationParameters(), out var validatedToken);

            // Additional validation to ensure the token is the type we are expecting
            if (validatedToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase
                ))
                throw new SecurityTokenException("Invalid token");

            return principal;
        };

        return getPrincipal.GetValue().Data;
    }

    private JwtSecurityToken GetJwtToken(User user, AccessToken accessToken)
    {
        var claims = GetClaims(user, accessToken);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationTimeInMinutes),
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