﻿using System;
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
        public virtual DbSet<UsersSession> UsersSessions { get; set; } = null!;

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
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("longUrl");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Urls)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__urls__userId__3A81B327");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "UQ__users__AB6E6164D9663D1C")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .HasMaxLength(38)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.EmailVerified).HasColumnName("emailVerified");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.GId)
                    .HasMaxLength(38)
                    .IsUnicode(false)
                    .HasColumnName("gId");

                entity.Property(e => e.LastName)
                    .HasMaxLength(30)
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
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("snType");
            });

            modelBuilder.Entity<UsersSession>(entity =>
            {
                entity.ToTable("users_session");

                entity.HasIndex(e => e.Token, "UQ__users_se__CA90DA7ABCE5EDAB")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.SessionEnd)
                    .HasColumnType("datetime")
                    .HasColumnName("session_end");

                entity.Property(e => e.SessionStart)
                    .HasColumnType("datetime")
                    .HasColumnName("session_start");

                entity.Property(e => e.Token)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("token");

                entity.Property(e => e.TokenValid).HasColumnName("token_valid");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.UsersSession)
                    .HasForeignKey<UsersSession>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__users_sessio__Id__5DCAEF64");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
