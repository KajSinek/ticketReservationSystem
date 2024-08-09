using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TRS.CoreApi.Database;

namespace HomeApp.ApiCore.Database
{
    public class ApplicationContextDesignTimeDbContextFactory
        : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BackgroundJobs.Api"))
                .AddJsonFile("appsettingsApp.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("HangfireConnection");

            builder.UseNpgsql(connectionString);

            return new AppDbContext(builder.Options);
        }
    }
}
