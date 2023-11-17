namespace LocalIdentity.SimpleInfra.Domain.Common;

public interface ISoftDeletedEntity : IEntity
{
    bool IsDeleted { get; set; }
    
    DateTimeOffset? DeletedDate { get; set; }
}