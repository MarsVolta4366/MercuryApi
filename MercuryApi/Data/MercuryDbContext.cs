using System;
using System.Collections.Generic;
using MercuryApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MercuryApi.Data;

public partial class MercuryDbContext : DbContext
{
    public MercuryDbContext()
    {
    }

    public MercuryDbContext(DbContextOptions<MercuryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:MercuryDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Project__3214EC0742969F99");

            entity.ToTable("Project");

            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Team).WithMany(p => p.Projects)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK__Project__TeamId__47DBAE45");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Team__3214EC079A3A0FF5");

            entity.ToTable("Team");

            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ticket__3214EC07099210D3");

            entity.ToTable("Ticket");

            entity.Property(e => e.Content)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Project).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__Ticket__ProjectI__4CA06362");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Ticket__UserId__4D94879B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC074821FE5F");

            entity.ToTable("User");

            entity.Property(e => e.Password)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasMany(d => d.Teams).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserTeam",
                    r => r.HasOne<Team>().WithMany()
                        .HasForeignKey("TeamId")
                        .HasConstraintName("FK__UserTeam__TeamId__49C3F6B7"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__UserTeam__UserId__48CFD27E"),
                    j =>
                    {
                        j.HasKey("UserId", "TeamId").HasName("PK__UserTeam__96AB623537E9A7E7");
                        j.ToTable("UserTeam");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
