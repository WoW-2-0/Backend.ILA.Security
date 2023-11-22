namespace LocalIdentity.SimpleInfra.Domain.Exceptions;

public class EntityConflictException : EntityException
{
    public EntityConflictException(Guid entityId, string message) : base(entityId, message)
    {
    }
}