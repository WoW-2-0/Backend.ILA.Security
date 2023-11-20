using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Application.Common.Querying;
using Microsoft.AspNetCore.Mvc;

namespace LocalIdentity.SimpleInfra.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController() : ControllerBase
{
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