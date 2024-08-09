using BackgroundJobs.Core.Database;
using GuardNet;
using static BackgroundJobs.Core.BackgroundJobsConstants.ConnectionStrings;

namespace BackgroundJobs.Core.Extensions;

public static class WebApplicationExtensions
{
    public static void AddCustomHangfireDatabase(this WebApplicationBuilder builder)
    {
        Guard.NotNull(builder, nameof(builder));

        EntityFrameworkCoreOptions efCore = builder.ConfigureOptions<EntityFrameworkCoreOptions>(
            GetSettingsFullPath(EntityFrameworkCoreSection)
        );
        string? connectionString = builder.Configuration.GetConnectionString(
            HangfireConnectionString
        );

        builder.Services.AddDbContext<HangfireContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                opts =>
                {
                    opts.MigrationsAssembly(efCore.MigrationAssembly);
                    opts.EnableRetryOnFailure(
                        maxRetryCount: efCore.MaxRetryCount,
                        maxRetryDelay: TimeSpan.FromSeconds(efCore.RetryDelayInSeconds),
                        null
                    );
                }
            );

            if (builder.Environment.IsLocal())
            {
                options.EnableSensitiveDataLogging().EnableDetailedErrors();
            }
        });
    }

    public static void AddCustomHangfireJobs(this WebApplicationBuilder builder)
    {
        Guard.NotNull(builder, nameof(builder));

        //builder.Services.AddTransient<OrdersDailyJob>();
    }
}
