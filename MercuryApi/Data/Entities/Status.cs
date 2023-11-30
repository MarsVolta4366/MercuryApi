using System;
using System.Collections.Generic;

namespace MercuryApi.Data.Entities;

public partial class Status
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
