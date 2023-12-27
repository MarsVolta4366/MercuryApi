namespace MercuryApi.Data.Upserts
{
    public class CommentUpsert
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public int TicketId { get; set; }
    }
}
