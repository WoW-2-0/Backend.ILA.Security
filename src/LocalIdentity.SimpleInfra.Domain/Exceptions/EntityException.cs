namespace LocalIdentity.SimpleInfra.Domain.Exceptions;

/// <summary>
/// Represents entity exception
/// </summary>
/// <param name="entityId">Id of entity</param>
/// <param name="message">Error message</param>
public class EntityException(Guid entityId, string message) : Exception;
