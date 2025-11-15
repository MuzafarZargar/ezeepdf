using System;
using System.Collections.Generic;

namespace EzeePdf.Core.DB;

public partial class LuProduct
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
