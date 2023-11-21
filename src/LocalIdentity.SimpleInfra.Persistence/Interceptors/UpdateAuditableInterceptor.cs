using LocalIdentity.SimpleInfra.Domain.Brokers.Interfaces;
using LocalIdentity.SimpleInfra.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LocalIdentity.SimpleInfra.Persistence.Interceptors;

public class UpdateAuditableInterceptor(IRequestUserContextProvider requestUserContextProvider) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        var auditableEntities = eventData.Context!.ChangeTracker.Entries<IAuditableEntity>().ToList();
        var creationAuditableEntities = eventData.Context!.ChangeTracker.Entries<ICreationAuditableEntity>().ToList();
        var modificationAuditableEntities = eventData.Context!.ChangeTracker.Entries<IModificationAuditableEntity>().ToList();
        var deletionAuditableEntities = eventData.Context!.ChangeTracker.Entries<IDeletionAuditableEntity>().ToList();

        auditableEntities.ForEach(
            entry =>
            {
                if (entry.State == EntityState.Added)
                    entry.Property(nameof(IAuditableEntity.CreatedTime)).CurrentValue = DateTimeOffset.UtcNow;

                if (entry.State == EntityState.Modified)
                    entry.Property(nameof(IAuditableEntity.ModifiedTime)).CurrentValue = DateTimeOffset.UtcNow;
            }
        );

        creationAuditableEntities.ForEach(
            entry =>
            {
                if (entry.State == EntityState.Added)
                    entry.Property(nameof(ICreationAuditableEntity.CreatedByUserId)).CurrentValue = requestUserContextProvider.GetUserId();
            }
        );

        modificationAuditableEntities.ForEach(
            entry =>
            {
                if (entry.State == EntityState.Modified)
                    entry.Property(nameof(IModificationAuditableEntity.ModifiedByUserId)).CurrentValue = requestUserContextProvider.GetUserId();
            }
        );

        deletionAuditableEntities.ForEach(
            entry =>
            {
                if (entry.State == EntityState.Deleted)
                    entry.Property(nameof(IDeletionAuditableEntity.DeletedByUserId)).CurrentValue = requestUserContextProvider.GetUserId();
            }
        );

        return base.SavingChangesAsync(eventData, result);
    }
}