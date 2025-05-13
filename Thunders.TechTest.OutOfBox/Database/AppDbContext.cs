using Microsoft.EntityFrameworkCore;
using Thunders.TechTest.Domain.Entities;

namespace Thunders.TechTest.OutOfBox.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TollTransactionEntity> TollTransaction { get; set; }
    public DbSet<ReportEntity> Report { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TollTransactionEntity>(entity =>
        {
            entity.HasIndex(t => t.DateTime);
            entity.HasIndex(t => t.PlazaId);
            entity.HasIndex(t => t.City);
            entity.HasIndex(t => t.State);
        });
    }
}