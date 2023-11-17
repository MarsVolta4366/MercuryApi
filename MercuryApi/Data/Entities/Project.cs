using System;
using System.Collections.Generic;

namespace MercuryApi.Data.Entities;

public partial class Project
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
