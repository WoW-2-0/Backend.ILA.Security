namespace LocalIdentity.SimpleInfra.Domain.Exceptions;

/// <summary>
/// Represents entity conflict exception
/// </summary>
/// <param name="entityId">Id of entity</param>
/// <param name="message">Error message</param>
public class EntityConflictException(Guid entityId, string message) : EntityException(entityId, message);