using FluentValidation;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Validators;

public class AccessTokenValidator : AbstractValidator<AccessToken>
{
    public AccessTokenValidator(IOptions<IdentityTokenSettings> jwtSettings)
    {
        var jwtSettingsValue = jwtSettings.Value;

        RuleSet(
            EntityEvent.OnCreate.ToString(),
            () =>
            {
                RuleFor(accessToken => accessToken.Id).NotEqual(Guid.Empty);
                
                RuleFor(accessToken => accessToken.Token).NotEmpty();
                
                RuleFor(accessToken => accessToken.UserId).NotEqual(Guid.Empty);
                
                RuleFor(accessToken => accessToken.ExpiryTime)
                    .GreaterThan(DateTimeOffset.UtcNow)
                    .Custom(
                        (accessToken, context) =>
                        {
                            if (accessToken > DateTimeOffset.UtcNow.AddMinutes(jwtSettingsValue.ExpirationTimeInMinutes))
                                context.AddFailure(
                                    nameof(AccessToken.ExpiryTime),
                                    $"{nameof(AccessToken.ExpiryTime)} cannot be greater than the expiration time of the JWT token."
                                );
                        }
                    );
            }
        );
    }
}