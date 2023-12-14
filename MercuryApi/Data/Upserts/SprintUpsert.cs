namespace MercuryApi.Data.Upserts
{
    public class SprintUpsert
    {
        public string Name { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int ProjectId { get; set; }
    }
}
