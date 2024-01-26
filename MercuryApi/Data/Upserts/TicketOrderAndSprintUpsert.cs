namespace MercuryApi.Data.Upserts
{
    public class TicketOrderAndSprintUpsert
    {
        public int TicketId { get; set; }
        public int NewOrderValue { get; set; }
        public int? NewSprintId { get; set; }
    }
}
