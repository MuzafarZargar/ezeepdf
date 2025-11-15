using System;
using System.Collections.Generic;

namespace EzeePdf.Core.DB;

public partial class User
{
    public int UserId { get; set; }

    public int UserTypeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool? Locked { get; set; }

    public DateTime? LockedAt { get; set; }

    public DateTime LastPasswordChangeTime { get; set; }

    public int FailedLoginAttemptCount { get; set; }

    public bool ForcePasswordChange { get; set; }

    public virtual ICollection<ErrorLog> ErrorLogs { get; set; } = new List<ErrorLog>();

    public virtual ICollection<Feedback> FeedbackActionTakenBies { get; set; } = new List<Feedback>();

    public virtual ICollection<Feedback> FeedbackUsers { get; set; } = new List<Feedback>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<UserPdfUsage> UserPdfUsages { get; set; } = new List<UserPdfUsage>();

    public virtual ICollection<PdfUpload> DeniedUserPdfUsages { get; set; } = new List<PdfUpload>();

    public virtual LuUserType UserType { get; set; } = null!;
}
