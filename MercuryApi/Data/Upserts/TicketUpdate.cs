namespace MercuryApi.Data.Upserts
{
    public class TicketUpdate
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int StatusId { get; set; }

        public int? SprintId { get; set; }

        public string Title { get; set; } = null!;

        public string? Content { get; set; }

        public int? Points { get; set; }
    }
}
