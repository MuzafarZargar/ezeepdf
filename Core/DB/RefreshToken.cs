using System;
using System.Collections.Generic;

namespace EzeePdf.Core.DB;

public partial class RefreshToken
{
    public int RefreshTokenId { get; set; }

    public int UserId { get; set; }

    public string SourceDevice { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime IssuedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public string IpAddress { get; set; } = null!;

    public int? ReplacedById { get; set; }

    public DateTime? GracePeriodExpiresAt { get; set; }

    public virtual ICollection<RefreshToken> InverseReplacedBy { get; set; } = new List<RefreshToken>();

    public virtual RefreshToken? ReplacedBy { get; set; }

    public virtual User User { get; set; } = null!;
}
