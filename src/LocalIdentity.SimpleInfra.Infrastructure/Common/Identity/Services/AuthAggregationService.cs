using System.Security.Authentication;
using AutoMapper;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Exceptions;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class AuthAggregationService(
    IMapper mapper,
    IPasswordGeneratorService passwordGeneratorService,
    IPasswordHasherService passwordHasherService,
    IAccountAggregatorService accountAggregatorService,
    // IUserSignInDetailsService userSignInDetailsService,
    // IAccessTokenGeneratorService accessTokenGeneratorService,
    // IAccessTokenService accessTokenService,
    IUserService userService,
    IRoleService roleService
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
    
    public async ValueTask<AccessToken> SignInAsync(SignInDetails signInDetails, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();

        // var foundUser = await userService.GetByEmailAddressAsync(signInDetails.EmailAddress,  cancellationToken: cancellationToken);
        //
        // if (foundUser is null || !passwordHasherService.HashPassword(foundUser.PasswordHash).Equals(foundUser.PasswordHash))
        //     throw new AuthenticationException("Invalid email address or password.");
        //
        // // Validate login location
        // var locationValidationResult = await userSignInDetailsService.ValidateSignInLocation(cancellationToken);
        //
        // // Notify user about changed location
        //
        // // Record login info
        // await userSignInDetailsService.RecordSignInAsync(false, cancellationToken);
        //
        // // Generate access token and save it
        // var tokenValue = accessTokenGeneratorService.GetToken(foundUser);
        // return await accessTokenService.CreateAsync(foundUser.Id, tokenValue.Token, tokenValue.ExpiryTime, true, cancellationToken);
    }
}