using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalIdentity.SimpleInfra.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet("{userId:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid userId)
    {
        var result = await userService.GetByIdAsync(userId);
        return result is not null ? Ok(result) : NotFound();
    }
}