using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Infra;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<UrlEncurtada> UrlEncurtada { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UrlEncurtada>().HasIndex(u => u.UrlCurta).IsUnique();
    }
}
