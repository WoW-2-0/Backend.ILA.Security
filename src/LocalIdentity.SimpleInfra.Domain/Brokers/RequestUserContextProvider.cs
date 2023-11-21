using System.Security.Authentication;
using LocalIdentity.SimpleInfra.Domain.Brokers.Interfaces;
using LocalIdentity.SimpleInfra.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace LocalIdentity.SimpleInfra.Domain.Brokers;

public class RequestUserContextProvider(IHttpContextAccessor httpContextAccessor) : IRequestUserContextProvider
{
    public Guid GetUserId()
    {
        var httpContext = httpContextAccessor.HttpContext!;
        var userIdClaim = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimConstants.UserId)?.Value;
        return userIdClaim is not null ? Guid.Parse(userIdClaim) : throw new AuthenticationException("User is not authenticated");
    }
}