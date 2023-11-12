using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Upserts;

namespace MercuryApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings.
            CreateMap<UserUpsert, User>();
            CreateMap<User, UserDto>().ReverseMap();

            // Team mappings.
            CreateMap<TeamUpsert, Team>()
                .ForMember(t => t.Users, opt => opt.Ignore());
        }
    }
}
