namespace LocalIdentity.SimpleInfra.Domain.Common;

public interface IModificationAuditableEntity
{
    Guid? ModifiedBy { get; set; }
}