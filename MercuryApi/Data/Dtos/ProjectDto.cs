namespace MercuryApi.Data.Dtos
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public string Name { get; set; } = null!;
    }
}
