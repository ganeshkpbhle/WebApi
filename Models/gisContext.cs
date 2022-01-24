using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApi.Models
{
    public partial class gisContext : DbContext
    {
        public gisContext()
        {
        }

        public gisContext(DbContextOptions<gisContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Url> Urls { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Url>(entity =>
            {
                entity.ToTable("urls");

                entity.Property(e => e.UrlId)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("urlId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_Date");

                entity.Property(e => e.LongUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("longUrl");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Urls)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__urls__userId__02FC7413");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "UQ__users__AB6E61649C60E046")
                    .IsUnique();

                entity.HasIndex(e => e.GId, "UQ__users__DCD90921AD85BF08")
                    .IsUnique();

                entity.HasIndex(e => e.GId, "uniq")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "unq")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.EmailVerified).HasColumnName("emailVerified");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.GId)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("gId");

                entity.Property(e => e.LastName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("mobile");

                entity.Property(e => e.Passwd)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("passwd");

                entity.Property(e => e.SnType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("snType");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
