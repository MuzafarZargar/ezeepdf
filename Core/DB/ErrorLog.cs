using System;
using System.Collections.Generic;

namespace EzeePdf.Core.DB;

public partial class ErrorLog
{
    public int ErrorLogId { get; set; }

    public int? UserId { get; set; }

    public DateTime DateCreated { get; set; }

    public string ErrorMessage { get; set; } = null!;

    public string FullMessage { get; set; } = null!;

    public string IpAddress { get; set; } = null!;

    public string ServerName { get; set; } = null!;
    
    public string Url { get; set; } = null!;

    public string? Source { get; set; }

    public virtual User? User { get; set; }
}
