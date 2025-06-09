using System;
using System.Collections.Generic;

namespace HealMe.Models;

public partial class ProductFeedback
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public int Rating { get; set; }

    public string FeedbackText { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }
}
