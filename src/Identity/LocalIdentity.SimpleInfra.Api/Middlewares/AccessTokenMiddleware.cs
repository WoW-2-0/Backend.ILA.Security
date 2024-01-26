using System.Security.Authentication;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Constants;

namespace LocalIdentity.SimpleInfra.Api.Middlewares;

public class AccessTokenMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var identitySecurityTokenService = context.RequestServices.GetRequiredService<IIdentitySecurityTokenService>();

        var accessTokenIdValue = context.User.Claims.FirstOrDefault(claim => claim.Type == ClaimConstants.AccessTokenId)?.Value;
        if (accessTokenIdValue != null)
        {
            var accessTokenId = Guid.Parse(accessTokenIdValue);
            _ = await identitySecurityTokenService.GetAccessTokenByIdAsync(accessTokenId, context.RequestAborted) ??
                throw new AuthenticationException("Access token not found");
        }

        await next(context);
    }
}