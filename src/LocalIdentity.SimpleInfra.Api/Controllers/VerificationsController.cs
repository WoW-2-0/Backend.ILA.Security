using LocalIdentity.SimpleInfra.Application.Common.Verifications.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocalIdentity.SimpleInfra.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VerificationsController : ControllerBase
{
    [HttpPut("{code}")]
    public async ValueTask<IActionResult> Verify(
        [FromRoute] string code,
        [FromServices] IVerificationProcessingService verificationProcessingService,
        CancellationToken cancellationToken
    )
    {
        var result = await verificationProcessingService.Verify(code, cancellationToken);
        return result ? Ok() : BadRequest();
    }
}