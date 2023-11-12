namespace MercuryApi.Data.Dtos
{
    public class TeamDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
