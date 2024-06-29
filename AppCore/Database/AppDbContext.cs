using AppCore.Entities;
using Microsoft.EntityFrameworkCore;
using AppCore.Database.EntityTypeConfiguration;

namespace AppCore.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<AccountBalance> AccountBalances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new TicketConfiguration());
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
    }
}
