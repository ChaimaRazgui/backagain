using Microsoft.EntityFrameworkCore;
using Naxxum.WeeCare.Authentification.Domain.Entities;

namespace Naxxum.WeeCare.Authentification.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    public DbSet<User> Users { get; set; }
}