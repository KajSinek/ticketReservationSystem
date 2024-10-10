using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BackgroundJobs.Core.Database;

public class HangfireContextDesignTimeDbContextFactory
    : IDesignTimeDbContextFactory<HangfireContext>
{
    public HangfireContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BackgroundJobs.Api"))
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<HangfireContext>();
        var connectionString = configuration.GetConnectionString("HangfireConnectionString");

        builder.UseNpgsql(connectionString);

        return new HangfireContext(builder.Options);
    }
}
