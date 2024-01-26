using System.Net;
using AutoMapper;
using LocalIdentity.SimpleInfra.Api.Models.Dtos;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Application.Common.Querying;
using LocalIdentity.SimpleInfra.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LocalIdentity.SimpleInfra.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMapper mapper, IAuthAggregationService authAggregationService) : ControllerBase
{
    #region Authentication

    [HttpPost("sign-up")]
    public async ValueTask<IActionResult> SignUp([FromBody] SignUpDetails signUpDetails, CancellationToken cancellationToken)
    {
        var result = await authAggregationService.SignUpAsync(signUpDetails, cancellationToken);
        return result ? Ok() : BadRequest();
    }

    [HttpPost("sign-in")]
    public async ValueTask<IActionResult> SignIn([FromBody] SignInDetails signInDetails, CancellationToken cancellationToken)
    {
        var result = await authAggregationService.SignInAsync(signInDetails, cancellationToken);
        return Ok(mapper.Map<IdentityTokenDto>(result));
    }

    [HttpPost("refresh-token")]
    public async ValueTask<IActionResult> RefreshToken([FromBody] string refreshTokenValue, CancellationToken cancellationToken)
    {
        var result = await authAggregationService.RefreshTokenAsync(refreshTokenValue, cancellationToken);
        return Ok(mapper.Map<AccessTokenDto>(result));
    }

    #endregion

    #region Roles

    [HttpGet("roles")]
    public async ValueTask<IActionResult> GetRoles(
        [FromQuery] FilterPagination paginationOptions,
        [FromServices] IRoleService roleService,
        CancellationToken cancellationToken
    )
    {
        var result = await roleService.GetByFilterAsync(paginationOptions, true, cancellationToken);
        return Ok(result);
    }

    #endregion
}