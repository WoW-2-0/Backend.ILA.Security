using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Constants;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class AccessTokenGeneratorService(IOptions<JwtSettings> jwtSettings) : IAccessTokenGeneratorService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public (string Token, DateTimeOffset ExpiryTime) GetToken(Guid accessTokenId, User user)
    {
        var jwtToken = GetJwtToken(accessTokenId, user);
        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return (token, jwtToken.ValidTo);
    }

    private JwtSecurityToken GetJwtToken(Guid accessTokenId, User user)
    {
        var claims = GetClaims(accessTokenId, user);

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

    private List<Claim> GetClaims(Guid accessTokenId, User user)
    {
        return new List<Claim>()
        {
            new(ClaimTypes.Email, user.EmailAddress),
            new(ClaimTypes.Role, user.Role!.Type.ToString()),
            new(ClaimConstants.UserId, user.Id.ToString()),
            new(ClaimConstants.AccessTokenId, accessTokenId.ToString()),
        };
    }
}