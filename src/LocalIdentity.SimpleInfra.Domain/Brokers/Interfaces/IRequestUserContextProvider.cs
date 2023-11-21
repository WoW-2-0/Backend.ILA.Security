namespace LocalIdentity.SimpleInfra.Domain.Brokers.Interfaces;

public interface IRequestUserContextProvider
{
    Guid GetUserId();
}