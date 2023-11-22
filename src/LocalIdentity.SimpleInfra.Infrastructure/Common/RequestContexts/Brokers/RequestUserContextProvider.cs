using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Brokers;
using LocalIdentity.SimpleInfra.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.RequestContexts.Brokers;

public class RequestUserContextProvider(IHttpContextAccessor httpContextAccessor, IUserService userService) : IRequestUserContextProvider
{
    public async ValueTask<Guid> GetUserIdAsync(CancellationToken cancellationToken = default)
    {
        var httpContext = httpContextAccessor.HttpContext!;
        var userIdClaim = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimConstants.UserId)?.Value;
        return userIdClaim is not null ? Guid.Parse(userIdClaim) : (await userService.GetSystemUserAsync(true, cancellationToken)).Id;
    }
}