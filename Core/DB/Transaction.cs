using System;
using System.Collections.Generic;

namespace EzeePdf.Core.DB;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int TransactionStatusId { get; set; }

    public int TransactionModeId { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int SubscriptionTypeId { get; set; }

    public decimal AmountPaid { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string IpAddress { get; set; } = null!;

    public string? BankReferenceId { get; set; }

    public string? Id2 { get; set; }

    public string? Id3 { get; set; }

    public string? BankMessage { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual LuProduct Product { get; set; } = null!;

    public virtual LuSubscriptionType SubscriptionType { get; set; } = null!;

    public virtual LuTransactionMode TransactionMode { get; set; } = null!;

    public virtual LuTransactionStatus TransactionStatus { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
}
