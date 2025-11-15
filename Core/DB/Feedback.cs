using System;
using System.Collections.Generic;

namespace EzeePdf.Core.DB;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? UserId { get; set; }

    public string FeedbackText { get; set; } = null!;

    public string? EmailAddress { get; set; }

    public DateTime DateCreated { get; set; }

    public string? SourceDevice { get; set; }

    public string? IpAddress { get; set; }

    public string? ActionTaken { get; set; }

    public int? ActionTakenById { get; set; }

    public DateTime? ActionDate { get; set; }

    public virtual User? ActionTakenBy { get; set; }

    public virtual User? User { get; set; }
}
