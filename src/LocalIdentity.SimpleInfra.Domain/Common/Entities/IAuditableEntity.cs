namespace LocalIdentity.SimpleInfra.Domain.Common.Entities;

public interface IAuditableEntity : IEntity
{
    DateTimeOffset CreatedTime { get; set; }
    
    DateTimeOffset? ModifiedTime { get; set; }
}