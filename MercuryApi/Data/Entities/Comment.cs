using System;
using System.Collections.Generic;

namespace MercuryApi.Data.Entities;

public partial class Comment
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime Date { get; set; }

    public int UserId { get; set; }

    public int TicketId { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
