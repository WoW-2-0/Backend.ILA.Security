namespace LocalIdentity.SimpleInfra.Domain.Common;

public abstract class SoftDeletedEntity : Entity, ISoftDeletedEntity
{
    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }
}