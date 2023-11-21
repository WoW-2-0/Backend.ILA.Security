using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Api.Data;

public static class SeedDataExtensions
{
    public static async ValueTask InitializeSeedAsync(this IServiceProvider serviceProvider)
    {
        var identityDbContext = serviceProvider.GetRequiredService<IdentityDbContext>();

        if (!await identityDbContext.Users.AnyAsync())
            await identityDbContext.SeedUsersAsync();
    }

    private static async ValueTask SeedUsersAsync(this IdentityDbContext identityDbContext)
    {
        if (await identityDbContext.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.Role!.Type == RoleType.System) is not null)
            return;

        var systemRoleId = await identityDbContext.Roles.Where(role => role.Type == RoleType.System).Select(role => role.Id).FirstOrDefaultAsync();
        await identityDbContext.Users.AddAsync(
            new User
            {
                FirstName = "System",
                LastName = string.Empty,
                Age = 0,
                EmailAddress = string.Empty,
                PasswordHash = string.Empty,
                CreatedTime = DateTimeOffset.UtcNow,
                RoleId = systemRoleId
            }
        );
        await identityDbContext.SaveChangesAsync();
    }
}