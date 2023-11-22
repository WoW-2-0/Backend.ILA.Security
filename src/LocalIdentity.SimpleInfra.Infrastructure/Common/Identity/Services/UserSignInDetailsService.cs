using System.Formats.Asn1;
using System.Security.Authentication;
using AutoMapper;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Application.Common.RequestContexts.Brokers;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class UserSignInDetailsService(
    IMapper mapper,
    IUserSignInDetailsRepository userSignInDetailsRepository,
    IRequestContextProvider requestContextProvider
) : IUserSignInDetailsService
{
    public async ValueTask<bool> ValidateSignInLocation(CancellationToken cancellationToken = default)
    {
        var connectionInfo = requestContextProvider.GetRequestContext();
        if (!connectionInfo.UserId.HasValue || connectionInfo.UserId.Value == Guid.Empty)
            throw new AuthenticationException("User is not authenticated.");

        var lastSignIn = await GetLastSignInDetailsAsync(connectionInfo.UserId.Value, true, cancellationToken);
        return lastSignIn is null || connectionInfo.IpAddress.Equals(lastSignIn.IpAddress);
    }

    public async ValueTask<UserSignInDetails?> GetLastSignInDetailsAsync(
        Guid userId,
        bool asNoTracking,
        CancellationToken cancellationToken = default
    )
    {
        return await userSignInDetailsRepository.Get(signInDetails => signInDetails.UserId == userId, asNoTracking: asNoTracking)
            .OrderByDescending(signInDetails => signInDetails.CreatedTime)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async ValueTask RecordSignInAsync(bool saveChanges, CancellationToken cancellationToken = default)
    {
        var connectionInfo = requestContextProvider.GetRequestContext();
        var signInDetails = mapper.Map<UserSignInDetails>(connectionInfo);

        await userSignInDetailsRepository.CreateAsync(signInDetails, saveChanges, cancellationToken);
    }
}