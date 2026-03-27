using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Storage;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<JobPost> JobPosts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<JobPost>()
            .HasKey(j => j.Id);

        modelBuilder.Entity<Application>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.User)
            .WithMany(u => u.Applications)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.JobPost)
            .WithMany(j => j.Applications)
            .HasForeignKey(a => a.JobPostId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}