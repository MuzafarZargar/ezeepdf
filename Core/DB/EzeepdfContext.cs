using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EzeePdf.Core.DB;

public partial class EzeepdfContext : DbContext
{
    public EzeepdfContext()
    {
    }

    public EzeepdfContext(DbContextOptions<EzeepdfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<LuPdfFunction> LuPdfFunctions { get; set; }

    public virtual DbSet<LuProduct> LuProducts { get; set; }

    public virtual DbSet<LuSetting> LuSettings { get; set; }

    public virtual DbSet<LuSubscriptionType> LuSubscriptionTypes { get; set; }

    public virtual DbSet<LuTransactionMode> LuTransactionModes { get; set; }

    public virtual DbSet<LuTransactionStatus> LuTransactionStatuses { get; set; }

    public virtual DbSet<LuUserType> LuUserTypes { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPdfUsage> UserPdfUsages { get; set; }
    
    public virtual DbSet<PdfUpload> PdfUploads { get; set; }
    
    public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.ErrorLogId).HasName("PK__ErrorLog__D65247C238D3D64E");

            entity.ToTable("ErrorLog");

            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.FullMessage).IsUnicode(false);
            entity.Property(e => e.IpAddress)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Source)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.ErrorLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ErrorLog__UserId__6383C8BA");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDD6E81AEECE");

            entity.ToTable("Feedback");

            entity.Property(e => e.ActionTaken).HasMaxLength(500);
            entity.Property(e => e.FeedbackText).HasMaxLength(500);

            entity.HasOne(d => d.ActionTakenBy).WithMany(p => p.FeedbackActionTakenBies)
                .HasForeignKey(d => d.ActionTakenById)
                .HasConstraintName("FK__Feedback__Action__70DDC3D8");

            entity.HasOne(d => d.User).WithMany(p => p.FeedbackUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Feedback__UserId__6FE99F9F");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoice__D796AAB56318BA62");

            entity.ToTable("Invoice");

            entity.HasIndex(e => e.InvoiceNumber, "UQ__Invoice__D776E981267D11D2").IsUnique();

            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Transaction).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoice__Transac__59063A47");
        });

        modelBuilder.Entity<LuPdfFunction>(entity =>
        {
            entity.HasKey(e => e.PdfFunctionId).HasName("PK__LuPdfFun__B597C564308EA981");

            entity.ToTable("LuPdfFunction");

            entity.Property(e => e.PdfFunctionId).ValueGeneratedNever();
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LuProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__LuProduc__B40CC6CD9C7D97EA");

            entity.ToTable("LuProduct");

            entity.Property(e => e.ProductId).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LuSetting>(entity =>
        {
            entity.HasKey(e => e.SettingsId).HasName("PK__LuSettin__991B19FCABCC696B");

            entity.Property(e => e.SettingsId).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Value)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LuSubscriptionType>(entity =>
        {
            entity.HasKey(e => e.SubscriptionTypeId).HasName("PK__LuSubscr__AFE5EEE8A72BF011");

            entity.ToTable("LuSubscriptionType");

            entity.Property(e => e.SubscriptionTypeId).ValueGeneratedNever();
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FeeAmount).HasColumnType("money");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LuTransactionMode>(entity =>
        {
            entity.HasKey(e => e.TransactionModeId).HasName("PK__LuTransa__C9BBD477F3E9C70E");

            entity.ToTable("LuTransactionMode");

            entity.Property(e => e.TransactionModeId).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LuTransactionStatus>(entity =>
        {
            entity.HasKey(e => e.TransactionStatusId).HasName("PK__LuTransa__57B5E183CD00D5FD");

            entity.ToTable("LuTransactionStatus");

            entity.Property(e => e.TransactionStatusId).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LuUserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PK__LuUserTy__40D2D81602AFD0E7");

            entity.ToTable("LuUserType");

            entity.Property(e => e.UserTypeId).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId).HasName("PK__RefreshT__F5845E39145EA61A");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.IpAddress)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SourceDevice)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Token)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.ReplacedBy).WithMany(p => p.InverseReplacedBy)
                .HasForeignKey(d => d.ReplacedById)
                .HasConstraintName("FK__RefreshTo__Repla__4E88ABD4");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefreshTo__UserI__4D94879B");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6BE2C5E6F7");

            entity.Property(e => e.AmountPaid).HasColumnType("money");
            entity.Property(e => e.BankMessage)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.BankReferenceId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IpAddress)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Produ__5441852A");

            entity.HasOne(d => d.SubscriptionType).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.SubscriptionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Subsc__5535A963");

            entity.HasOne(d => d.TransactionMode).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.TransactionModeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Trans__52593CB8");

            entity.HasOne(d => d.TransactionStatus).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.TransactionStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Trans__5165187F");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__UserI__534D60F1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CDB884791");

            entity.Property(e => e.EmailAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastPasswordChangeTime).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Locked).HasDefaultValue(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__UserTypeI__46E78A0C");
        });

        modelBuilder.Entity<UserPdfUsage>(entity =>
        {
            entity.HasKey(e => e.UserPdfUsageId).HasName("PK__UserPdfU__25D0CD0F30AA5B4A");

            entity.ToTable("UserPdfUsage");

            entity.Property(e => e.IpAddress)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.SourceDevice)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.PdfFunction).WithMany(p => p.UserPdfUsages)
                .HasForeignKey(d => d.PdfFunctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserPdfUs__PdfFu__74AE54BC");

            entity.HasOne(d => d.User).WithMany(p => p.UserPdfUsages)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserPdfUs__UserI__73BA3083");

            entity.HasOne(d => d.UserSubscription).WithMany(p => p.UserPdfUsages)
                .HasForeignKey(d => d.UserSubscriptionId)
                .HasConstraintName("FK__UserPdfUs__UserS__75A278F5");
        });

        modelBuilder.Entity<PdfUpload>(entity =>
        {
            entity.HasKey(e => e.PdfUploadId).HasName("PK__PdfUpl__41D0CD0F30AA5B4A");

            entity.ToTable("PdfUpload");

            entity.Property(e => e.IpAddress)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.Property(e => e.SourceDevice)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.DeniedUserPdfUsages)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserPdfDnu__UserI__73BA308A");

            entity.HasOne(d => d.PdfFunction).WithMany(p => p.PdfUploads)
                .HasForeignKey(d => d.PdfFunctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PdfUpl__PdfFu__711D54CA");
        }); 
        
        modelBuilder.Entity<UserSubscription>(entity =>
        {
            entity.HasKey(e => e.UserSubscriptionId).HasName("PK__UserSubs__D1FD777CCDC23DA5");

            entity.ToTable("UserSubscription");

            entity.Property(e => e.SourceDevice)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Transaction).WithMany(p => p.UserSubscriptions)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserSubsc__Trans__5BE2A6F2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
