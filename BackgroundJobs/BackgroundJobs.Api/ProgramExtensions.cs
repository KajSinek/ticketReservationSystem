using Abstractions;
using BackgroundJobs.Core.Extensions;
using Hangfire;
using Hangfire.Extensions.ApplicationInsights;
using Hangfire.PostgreSql;
using Hangfire.RecurringJobAdmin;
using static BackgroundJobs.Core.BackgroundJobsConstants.ConnectionStrings;

namespace BackgroundJobs.Api;

public static class ProgramExtensions
{
    public static void AddCustomServices(this IServiceCollection serviceCollection)
    {
        var assembly = typeof(ProgramExtensions).Assembly;
        var services = assembly
            .GetTypes()
            .Where(type => type.IsClass && type.IsAssignableFrom(typeof(IService)));
        foreach (var service in services)
        {
            serviceCollection.AddTransient(service);
        }
    }

    public static void AddCustomHangfire(this WebApplicationBuilder builder)
    {
        string? connectionString =
            builder.Configuration.GetConnectionString(HangfireConnectionString)
            ?? throw new InvalidOperationException(
                "Failed to register Hangfire service, missing connection string"
            );

        builder.Services.AddHangfireApplicationInsights();
        builder.Services.AddHangfire(configuration =>
            configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(factory => factory.UseNpgsqlConnection(connectionString))
                .UseRecurringJobAdmin(typeof(ProgramExtensions).Assembly)
        );

        builder.Services.AddHangfireServer(options =>
        {
            options.HeartbeatInterval = new TimeSpan(0, 1, 0);
            options.ServerCheckInterval = new TimeSpan(0, 1, 0);
            options.SchedulePollingInterval = new TimeSpan(0, 1, 0);
        });

        builder.AddCustomHangfireDatabase();
        builder.AddHangfireJobProvider();
        builder.AddCustomHangfireJobs();
    }
}
