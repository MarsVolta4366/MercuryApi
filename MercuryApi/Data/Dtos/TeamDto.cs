namespace MercuryApi.Data.Dtos
{
    public class TeamDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<ProjectDto> Projects { get; set; } = new List<ProjectDto>();

        public virtual ICollection<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
