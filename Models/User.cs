using System;
using System.Collections.Generic;

namespace HealMe.Models;

public partial class User
{
    public long UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
