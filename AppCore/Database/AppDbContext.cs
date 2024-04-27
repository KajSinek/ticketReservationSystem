using AppCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}
