using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

public interface IAccessTokenGeneratorService
{
    (string Token, DateTimeOffset ExpiryTime) GetToken(Guid accessTokenId, User user);
}