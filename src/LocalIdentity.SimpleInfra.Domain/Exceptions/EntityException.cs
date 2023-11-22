namespace LocalIdentity.SimpleInfra.Domain.Exceptions;

public class EntityException : Exception
{
    public EntityException(Guid entityId, string message)
    {
    }
}