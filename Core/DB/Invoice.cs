using System;
using System.Collections.Generic;

namespace EzeePdf.Core.DB;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public DateTime InvoiceDate { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public int TransactionId { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;
}
