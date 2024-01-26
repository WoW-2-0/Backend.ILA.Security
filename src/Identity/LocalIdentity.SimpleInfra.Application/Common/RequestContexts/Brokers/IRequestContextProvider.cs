namespace LocalIdentity.SimpleInfra.Application.Common.RequestContexts.Brokers;

public interface IRequestContextProvider
{
    Models.RequestContext GetRequestContext();
}