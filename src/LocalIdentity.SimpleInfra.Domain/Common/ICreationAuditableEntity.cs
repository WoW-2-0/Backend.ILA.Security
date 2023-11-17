namespace LocalIdentity.SimpleInfra.Domain.Common;

public interface ICreationAuditableEntity
{
    Guid CreatedBy { get; set; }
}