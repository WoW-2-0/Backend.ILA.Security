using System.Linq.Expressions;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public IQueryable<User> Get(Expression<Func<User, bool>>? predicate = default, bool asNoTracking = false)
    {
        return userRepository.Get(predicate, asNoTracking);
    }

    public ValueTask<User?> GetByIdAsync(Guid userId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        return userRepository.GetByIdAsync(userId, asNoTracking, cancellationToken);
    }

    public async ValueTask<User?> GetByEmailAddressAsync(
        string emailAddress,
        bool asNoTracking = false,
        CancellationToken cancellationToken = default
    )
    {
        return await userRepository.Get(asNoTracking: asNoTracking)
            .SingleOrDefaultAsync(user => user.EmailAddress == emailAddress, cancellationToken: cancellationToken);
    }

    public async Task<Guid?> CheckUserByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken = default)
    {
        return await Get(user => user.EmailAddress == emailAddress, true).Select(user => user.Id).SingleOrDefaultAsync(cancellationToken);
    }

    public ValueTask<User> CreateAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return userRepository.CreateAsync(user, saveChanges, cancellationToken);
    }

    public ValueTask<User> UpdateAsync(User user, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return userRepository.UpdateAsync(user, saveChanges, cancellationToken);
    }
}