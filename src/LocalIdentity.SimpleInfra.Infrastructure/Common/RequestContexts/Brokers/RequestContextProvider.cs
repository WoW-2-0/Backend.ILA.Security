using System.Net.Http.Headers;
using LocalIdentity.SimpleInfra.Application.Common.RequestContexts.Brokers;
using LocalIdentity.SimpleInfra.Application.Common.RequestContexts.Models;
using LocalIdentity.SimpleInfra.Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.RequestContexts.Brokers;

public class RequestContextProvider(IHttpContextAccessor httpContextAccessor) : IRequestContextProvider
{
    public RequestContext GetRequestContext()
    {
        var httpContext = httpContextAccessor.HttpContext!;
        var userIdClaim = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimConstants.UserId)?.Value;

        var requestContext = new RequestContext
        {
            UserId = userIdClaim is not null ? Guid.Parse(userIdClaim) : default,
            IpAddress = httpContext.Connection.RemoteIpAddress!.ToString(),
            UserAgent = httpContext.Request.Headers[HeaderNames.UserAgent].ToString()
        };

        return requestContext;
    }
}