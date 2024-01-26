using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

public interface IAuthAggregationService
{
    ValueTask<bool> SignUpAsync(SignUpDetails signUpDetails, CancellationToken cancellationToken = default);

    ValueTask<(AccessToken AccessToken, RefreshToken RefreshToken)> SignInAsync(
        SignInDetails signInDetails,
        CancellationToken cancellationToken = default
    );

    ValueTask<AccessToken> RefreshTokenAsync(string refreshTokenValue, CancellationToken cancellationToken = default);
}