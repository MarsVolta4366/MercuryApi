using System;
using System.Collections.Generic;

namespace MercuryApi.Data.Entities;

public partial class Ticket
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int? UserId { get; set; }

    public int StatusId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual User? User { get; set; }
}
