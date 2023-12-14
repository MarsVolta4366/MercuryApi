namespace MercuryApi.Data.Dtos
{
    public class TicketDto
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int? UserId { get; set; }

        public int StatusId { get; set; }

        public int? SprintId { get; set; }

        public string SprintName { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Content { get; set; }

        public int? Points { get; set; }

        public StatusDto Status { get; set; } = null!;

        public UserDto? User { get; set; }
    }
}
