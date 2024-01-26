using LocalIdentity.SimpleInfra.Domain.Common.Entities;

namespace LocalIdentity.SimpleInfra.Domain.Exceptions;

/// <summary>
/// Represents entity not found exception
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class EntityNotFoundException<TEntity>(Guid entityId) : EntityException(entityId, $"{typeof(TEntity).FullName} with id {entityId} not found.")
    where TEntity : IEntity;