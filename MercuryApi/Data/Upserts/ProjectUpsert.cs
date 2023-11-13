namespace MercuryApi.Data.Upserts
{
    public class ProjectUpsert
    {
        public int TeamId { get; set; }

        public string Name { get; set; } = null!;
    }
}
