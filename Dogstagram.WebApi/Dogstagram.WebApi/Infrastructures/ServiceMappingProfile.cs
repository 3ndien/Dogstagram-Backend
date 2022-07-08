
namespace Dogstagram.WebApi.Infrastructures
{
    using AutoMapper;
    using Dogstagram.WebApi.Data.Models;
    using Dogstagram.WebApi.Features.Identity.Models;

    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            this.CreateMap<RegisterRequestModel, User>();
        }
    }
}
