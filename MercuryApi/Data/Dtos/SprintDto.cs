using MercuryApi.Helpers;
using System.Text.Json.Serialization;

namespace MercuryApi.Data.Dtos
{
    public class SprintDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime StartDate { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime EndDate { get; set; }

        public int ProjectId { get; set; }

        public virtual ICollection<TicketDto> Tickets { get; set; } = new List<TicketDto>();
    }
}
