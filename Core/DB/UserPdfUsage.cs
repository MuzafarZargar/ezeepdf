using System;
using System.Collections.Generic;

namespace EzeePdf.Core.DB;

public partial class UserPdfUsage
{
    public int UserPdfUsageId { get; set; }

    public int? UserId { get; set; }

    public int PdfFunctionId { get; set; }

    public int? UserSubscriptionId { get; set; }

    public DateTime UsageDate { get; set; }

    public int PdfPageCount { get; set; }

    public double PdfSize { get; set; }

    public double PdfChangedSize { get; set; }
    
    public string IpAddress { get; set; } = null!;

    public string SourceDevice { get; set; } = null!;

    public virtual LuPdfFunction PdfFunction { get; set; } = null!;

    public virtual User? User { get; set; }

    public virtual UserSubscription? UserSubscription { get; set; }
}
