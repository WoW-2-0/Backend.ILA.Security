using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

public interface IAccessTokenGeneratorService
{
    (string Token, DateTimeOffset ExpiryTime) GetToken(User user);

    JwtSecurityToken GetJwtToken(User user);

    List<Claim> GetClaims(User user);
}