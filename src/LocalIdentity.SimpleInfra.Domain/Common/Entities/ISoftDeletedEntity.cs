namespace LocalIdentity.SimpleInfra.Domain.Common.Entities;

public interface ISoftDeletedEntity : IEntity
{
    bool IsDeleted { get; set; }
    
    DateTimeOffset? DeletedDate { get; set; }
}