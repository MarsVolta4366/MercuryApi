namespace MercuryApi.Data.Dtos
{
    public class TicketDto
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int? UserId { get; set; }

        public string Title { get; set; } = null!;

        public string? Content { get; set; }

        public UserDto? User { get; set; }
    }
}
