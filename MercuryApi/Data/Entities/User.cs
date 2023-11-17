using System;
using System.Collections.Generic;

namespace MercuryApi.Data.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
