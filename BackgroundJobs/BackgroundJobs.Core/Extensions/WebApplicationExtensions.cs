using Abstractions.BackgroundJobs.Hangfire;
using BackgroundJobs.Core.Database;
using EntityFrameworkCore.BackgroundJobs.Hangfire;
using EntityFrameworkCore.Configuration;
using GuardNet;
using Microsoft.EntityFrameworkCore;
using VismaIdella.Vips.Abstractions.Extensions;
using static BackgroundJobs.Core.BackgroundJobsConstants.ConnectionStrings;

namespace BackgroundJobs.Core.Extensions;

public static class WebApplicationExtensions
{
    public static string GetSettingsFullPath(string name)
    {
        return $"Settings:{name}";
    }

    public const string EntityFrameworkCoreSection = "EntityFrameworkCore";

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
        });
    }

    public static void AddCustomHangfireJobs(this WebApplicationBuilder builder)
    {
        Guard.NotNull(builder, nameof(builder));

        //builder.Services.AddTransient<OrdersDailyJob>();
    }

    public static void AddHangfireJobProvider(this WebApplicationBuilder builder)
    {
        Guard.NotNull(builder, nameof(builder));

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<IHangfireJobProvider, HangfireJobProvider>();
    }
}
