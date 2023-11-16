using MercuryApi.Data.Dtos;

namespace MercuryApi.Data.Upserts
{
    public class TeamUpsert
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
