using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Application.Common.Verifications.Services;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Verifications.Services;

public class VerificationProcessingService
    (IUserInfoVerificationCodeService userInfoVerificationCodeService, IUserService userService) : IVerificationProcessingService
{
    public async ValueTask<bool> Verify(string code, CancellationToken cancellationToken)
    {
        var userActionVerificationCode = await userInfoVerificationCodeService.GetByCodeAsync(code, cancellationToken);

        if (!userActionVerificationCode.IsValid) return false;

        var user = await userService.GetByIdAsync(userActionVerificationCode.Code.UserId, cancellationToken: cancellationToken) ??
                   throw new InvalidOperationException();

        user.IsEmailAddressVerified = true;
        await userService.UpdateAsync(user, false, cancellationToken);
        await userInfoVerificationCodeService.DeactivateAsync(userActionVerificationCode.Code.Id, cancellationToken: cancellationToken);

        return true;
    }
}