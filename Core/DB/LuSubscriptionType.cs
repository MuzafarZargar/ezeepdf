using System;
using System.Collections.Generic;

namespace EzeePdf.Core.DB;

public partial class LuSubscriptionType
{
    public int SubscriptionTypeId { get; set; }

    public string Name { get; set; } = null!;

    public decimal FeeAmount { get; set; }

    public string? Description { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
