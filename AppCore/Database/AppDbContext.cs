using AppCore.Entities;
using Microsoft.EntityFrameworkCore;
using AppCore.Database.EntityTypeConfiguration;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Net;

namespace AppCore.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AccountConfiguration());
    }
}
