namespace LocalIdentity.SimpleInfra.Domain.Common.Entities;

public interface IModificationAuditableEntity
{
    Guid? ModifiedBy { get; set; }
}