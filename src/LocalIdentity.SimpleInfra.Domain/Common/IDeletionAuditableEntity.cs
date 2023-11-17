namespace LocalIdentity.SimpleInfra.Domain.Common;

public interface IDeletionAuditableEntity
{
    Guid? DeletedBy { get; set; }
}