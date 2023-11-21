namespace LocalIdentity.SimpleInfra.Domain.Common.Entities;

public interface ICreationAuditableEntity
{
    Guid CreatedBy { get; set; }
}