using System;
using System.Collections.Generic;

namespace HealMe.Models;

public partial class Task
{
    public long TaskId { get; set; }

    public long UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Note { get; set; }

    public string? ImageUrl { get; set; }

    public bool Completed { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
