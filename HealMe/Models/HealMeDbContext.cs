using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HealMe.Models;

public partial class HealMeDbContext : DbContext
{
    public HealMeDbContext()
    {
    }

    public HealMeDbContext(DbContextOptions<HealMeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ProductFeedback> ProductFeedbacks { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-PPCF4GS\\THANH;Database=HealMeDb;user=sa;password=1;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductFeedback>(entity =>
        {
            entity.HasIndex(e => e.CreatedAt, "IX_ProductFeedbacks_CreatedAt").IsDescending();

            entity.HasIndex(e => e.IsActive, "IX_ProductFeedbacks_IsActive");

            entity.HasIndex(e => e.Rating, "IX_ProductFeedbacks_Rating");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FeedbackText).HasMaxLength(1000);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tasks__7C6949B1DEFC8EDC");

            entity.Property(e => e.CompletedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Tasks_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CC84AED1E");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E416674E76").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
