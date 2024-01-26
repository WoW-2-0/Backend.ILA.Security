namespace LocalIdentity.SimpleInfra.Application.Common.RequestContexts.Models;

public class RequestContext
{
    public Guid? UserId { get; set; }
    
    public string IpAddress { get; set; } = default!;

    public string UserAgent { get; set; } = default!;
}