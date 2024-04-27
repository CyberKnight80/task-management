using Microsoft.EntityFrameworkCore;
using TaskManagement.Server.Models;

namespace TaskManagement.Server.Services;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Team> Teams { get; set; }

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
    }
}

