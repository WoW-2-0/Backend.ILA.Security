using AutoMapper;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Application.Common.RequestContexts.Models;
using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Mappers;

public class UserSignInDetailsMapper : Profile
{
    public UserSignInDetailsMapper()
    {
        CreateMap<RequestContext, UserSignInDetails>();
    }
}