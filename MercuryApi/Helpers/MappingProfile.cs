using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Upserts;

namespace MercuryApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User entity mappings.
            CreateMap<UserUpsert, User>();
            CreateMap<User, UserDto>().ReverseMap();

            // Team entity mappings.
            CreateMap<TeamUpsert, Team>()
                .ForMember(t => t.Users, opt => opt.Ignore());
            CreateMap<Team, TeamDto>();

            // Project entity mappings.
            CreateMap<ProjectUpsert, Project>();
            CreateMap<Project, ProjectDto>();

            // Ticket entity mappings.
            CreateMap<TicketCreate, Ticket>();
            CreateMap<TicketUpdate, Ticket>();
            CreateMap<Ticket, TicketDto>();

            // Status entity mappings.
            CreateMap<Status, StatusDto>();

            // Sprint entity mappings.
            CreateMap<SprintUpsert, Sprint>();
            CreateMap<Sprint, SprintDto>();

            // Comment entity mappings.
            CreateMap<CommentUpsert, Comment>();
            CreateMap<Comment, CommentDto>();
        }
    }
}
