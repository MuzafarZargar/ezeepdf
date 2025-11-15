using System;
using System.Collections.Generic;

namespace EzeePdf.Core.DB;

public partial class UserSubscription
{
    public int UserSubscriptionId { get; set; }

    public int TransactionId { get; set; }

    public string SourceDevice { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool Active { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;

    public virtual ICollection<UserPdfUsage> UserPdfUsages { get; set; } = new List<UserPdfUsage>();
}
