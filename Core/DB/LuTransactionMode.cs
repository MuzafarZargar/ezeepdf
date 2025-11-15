using System;
using System.Collections.Generic;

namespace EzeePdf.Core.DB;

public partial class LuTransactionMode
{
    public int TransactionModeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
