﻿namespace MercuryApi.Data.Dtos
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<SprintDto> Sprints { get; set; } = new List<SprintDto>();

        public virtual ICollection<TicketDto> Tickets { get; set; } = new List<TicketDto>();
    }
}
