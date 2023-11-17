namespace MercuryApi.Data.Upserts
{
    public class TicketUpsert
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int? UserId { get; set; }

        public string Title { get; set; } = null!;

        public string? Content { get; set; }
    }
}
