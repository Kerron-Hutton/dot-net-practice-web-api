using Microsoft.EntityFrameworkCore;
using SampleApp.Models;

namespace SampleApp.Data;

public class DataContext : DbContext
{
    public DbSet<SuperHero> SuperHeroes { get; set; } = null!;

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SuperHero>()
            .HasIndex(h => h.Name)
            .IsUnique();
    }
}