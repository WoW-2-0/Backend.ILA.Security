namespace LocalIdentity.SimpleInfra.Domain.Common.Entities;

public interface IDeletionAuditableEntity
{
    Guid? DeletedBy { get; set; }
}