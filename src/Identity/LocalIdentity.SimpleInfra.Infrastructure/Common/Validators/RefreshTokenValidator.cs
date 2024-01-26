using FluentValidation;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Validators;

public class RefreshTokenValidator : AbstractValidator<RefreshToken>
{
    public RefreshTokenValidator(IOptions<IdentityTokenSettings> identityTokenSettings)
    {
        var identityTokenSettingsValue = identityTokenSettings.Value;

        RuleSet(
            EntityEvent.OnCreate.ToString(),
            () =>
            {
                RuleFor(refreshToken => refreshToken.Id).NotEqual(Guid.Empty);

                RuleFor(refreshToken => refreshToken.Token).NotEmpty();

                RuleFor(refreshToken => refreshToken.UserId).NotEqual(Guid.Empty);

                RuleFor(refreshToken => refreshToken.ExpiryTime)
                    .GreaterThan(DateTimeOffset.UtcNow)
                    .Custom(
                        (refreshToken, context) =>
                        {
                            if (refreshToken >
                                DateTimeOffset.UtcNow.AddMinutes(identityTokenSettingsValue.RefreshTokenExtendedExpirationTimeInMinutes))
                                context.AddFailure(
                                    nameof(RefreshToken.ExpiryTime),
                                    $"{nameof(RefreshToken.ExpiryTime)} cannot be greater than the expiration time of the JWT token."
                                );
                        }
                    )
                    .When(refreshToken => refreshToken.EnableExtendedExpiryTime)
                    .Custom(
                        (refreshToken, context) =>
                        {
                            if (refreshToken > DateTimeOffset.UtcNow.AddMinutes(identityTokenSettingsValue.RefreshTokenExpirationTimeInMinutes))
                                context.AddFailure(
                                    nameof(RefreshToken.ExpiryTime),
                                    $"{nameof(RefreshToken.ExpiryTime)} cannot be greater than the expiration time of the JWT token."
                                );
                        }
                    )
                    .When(refreshToken => !refreshToken.EnableExtendedExpiryTime);
            }
        );
    }
}