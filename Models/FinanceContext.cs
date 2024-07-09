using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Finance.Models
{
    public partial class FinanceContext : DbContext
    {
        public FinanceContext()
        {
        }

        public FinanceContext(DbContextOptions<FinanceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.UserEmail });

                entity.ToTable("user");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(100)
                    .HasColumnName("user_email")
                    .IsFixedLength();

                entity.Property(e => e.UserDtcad)
                    .HasColumnName("user_dtcad")
                    .HasColumnType("date");

                entity.Property(e => e.UserName)
                    .IsUnicode(false)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserPasswd)
                    .IsUnicode(false)
                    .HasColumnName("user_passwd");
            });
        

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
