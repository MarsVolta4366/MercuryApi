using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Upserts;

namespace MercuryApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserUpsert, User>();
            CreateMap<User, UserDto>();
        }
    }
}
