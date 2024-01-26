using System.Security.Authentication;
using AutoMapper;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Brokers;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class AuthAggregationService(
    IMapper mapper,
    IPasswordGeneratorService passwordGeneratorService,
    IPasswordHasherService passwordHasherService,
    IAccountAggregatorService accountAggregatorService,
    // IUserSignInDetailsService userSignInDetailsService,
    IIdentitySecurityTokenGenerationService identitySecurityTokenGenerationService,
    IIdentitySecurityTokenService identitySecurityTokenService,
    IUserService userService,
    IRoleService roleService,
    IRequestUserContextProvider requestUserContextProvider
) : IAuthAggregationService
{
    public async ValueTask<bool> SignUpAsync(SignUpDetails signUpDetails, CancellationToken cancellationToken = default)
    {
        var foundUserId = await userService.GetIdByEmailAddressAsync(signUpDetails.EmailAddress, cancellationToken);

        if (foundUserId.HasValue)
            throw new EntityConflictException(foundUserId.Value, "User with this email address already exists.");

        // Hash password
        var user = mapper.Map<User>(signUpDetails);
        var password = signUpDetails.AutoGeneratePassword
            ? passwordGeneratorService.GeneratePassword()
            : passwordGeneratorService.GetValidatedPassword(signUpDetails.Password!, user);

        user.RoleId = await roleService.GetDefaultRoleId(cancellationToken);
        user.PasswordHash = passwordHasherService.HashPassword(password);

        // Create user
        return await accountAggregatorService.CreateUserAsync(user, cancellationToken);
    }

    public async ValueTask<(AccessToken AccessToken, RefreshToken RefreshToken)> SignInAsync(
        SignInDetails signInDetails,
        CancellationToken cancellationToken = default
    )
    {
        var foundUser = await userService.GetByEmailAddressAsync(signInDetails.EmailAddress, cancellationToken: cancellationToken);

        // check user
        if (foundUser is null || !passwordHasherService.ValidatePassword(signInDetails.Password, foundUser.PasswordHash))
            throw new AuthenticationException("Invalid email address or password.");

        if (!foundUser.IsEmailAddressVerified)
            throw new AuthenticationException("Email address is not verified.");

        return await CreateTokens(foundUser, cancellationToken);
    }

    public async ValueTask<AccessToken> RefreshTokenAsync(string refreshTokenValue, CancellationToken cancellationToken = default)
    {
        var accessTokenValue = requestUserContextProvider.GetAccessToken();

        if (string.IsNullOrWhiteSpace(refreshTokenValue))
            throw new ArgumentException("Invalid identity security token value", nameof(refreshTokenValue));

        if (string.IsNullOrWhiteSpace(accessTokenValue))
            throw new InvalidOperationException("Invalid identity security token value");

        // Check refresh token and access token
        var refreshToken = await identitySecurityTokenService.GetRefreshTokenByValueAsync(refreshTokenValue, cancellationToken);
        if (refreshToken is null)
            throw new AuthenticationException("Please login again.");

        var accessToken = identitySecurityTokenGenerationService.GetAccessToken(accessTokenValue);
        if (accessToken is null)
        {
            // Remove refresh token if access token is not valid
            await identitySecurityTokenService.RemoveRefreshTokenAsync(refreshTokenValue, cancellationToken);
            throw new InvalidOperationException("Invalid identity security token value");
        }

        // Remove refresh token and access token if user id is not same
        if (refreshToken.UserId != accessToken.UserId)
        {
            await identitySecurityTokenService.RemoveAccessTokenAsync(accessToken.Id, cancellationToken);
            await identitySecurityTokenService.RemoveRefreshTokenAsync(refreshTokenValue, cancellationToken);
            throw new AuthenticationException("Please login again.");
        }

        var foundUser =
            await userService
                .Get(user => user.Id == accessToken.UserId, true)
                .Include(user => user.Role)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
            throw new EntityNotFoundException<User>(accessToken.UserId);

        // Generate access token
        await identitySecurityTokenService.RemoveAccessTokenAsync(accessToken.Id, cancellationToken);
        accessToken = identitySecurityTokenGenerationService.GenerateAccessToken(foundUser);

        return await identitySecurityTokenService.CreateAccessTokenAsync(accessToken, cancellationToken: cancellationToken);
    }

    private async Task<(AccessToken AccessToken, RefreshToken RefreshToken)> CreateTokens(User user, CancellationToken cancellationToken = default)
    {
        // Generate access token
        var accessToken = identitySecurityTokenGenerationService.GenerateAccessToken(user);

        // Generate refresh token
        var refreshToken = identitySecurityTokenGenerationService.GenerateRefreshToken(user);

        // create token
        return (await identitySecurityTokenService.CreateAccessTokenAsync(accessToken, cancellationToken: cancellationToken),
            await identitySecurityTokenService.CreateRefreshTokenAsync(refreshToken, cancellationToken: cancellationToken));
    }
}