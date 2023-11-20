namespace MercuryApi.Data.Dtos
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<TicketDto> Tickets { get; set; } = new List<TicketDto>();
    }
}
