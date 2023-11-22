namespace LocalIdentity.SimpleInfra.Domain.Brokers;

public interface IRequestUserContextProvider
{
    ValueTask<Guid> GetUserIdAsync(CancellationToken cancellationToken = default);
}