using Microsoft.EntityFrameworkCore;
using TaskManagement.Server.Models;

namespace TaskManagement.Server.Services;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Team> Teams { get; set; }

    public DbSet<Models.Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=taskmanagement.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the many-to-many relationship
        modelBuilder.Entity<User>()
            .HasMany(u => u.Teams)
            .WithMany(t => t.Users)
            .UsingEntity(j => j.ToTable("UserTeams")); // Specify the name of the join table if desired

        // Configure the Name field in the Team entity to be unique
        modelBuilder.Entity<Team>()
            .HasIndex(t => t.Name)
            .IsUnique();

        // Configure Task entity
        modelBuilder.Entity<Models.Task>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                .IsRequired();

            entity.Property(t => t.CreatedAt)
                .IsRequired();

            entity.Property(t => t.Status)
                .IsRequired();

            // Configure the relationship between Task and User (Owner)
            entity.HasOne(t => t.Owner)
                .WithMany(u => u.OwnedTasks)
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship between Task and User (AssignedUser)
            entity.HasOne(t => t.AssignedUser)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure the relationship between Task and Team
            entity.HasOne(t => t.Team)
                .WithMany(t => t.Tasks)
                .HasForeignKey(t => t.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}

